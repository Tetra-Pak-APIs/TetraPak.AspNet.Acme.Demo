using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using demo.Acme.Models;
using Microsoft.Extensions.Logging;
using TetraPak;
using TetraPak.AspNet;
using TetraPak.Logging;

namespace demo.Acme.Repositories
{
    /// <summary>
    ///   A simple memory-based repository, for demonstraion/POC purposes.
    /// </summary>
    /// <typeparam name="T">
    ///   The type of items supported by the repository.
    /// </typeparam>
    public abstract class Repository<T> : IRepository<T> where T : Model
    {
        IDictionary<string, T> _items;
        string? _itemTypeName;
        uint _delayMax;
        uint _delayMin;

        ILogger? Logger { get; }

        /// <summary>
        ///   Gets the name of the item type supported by this repository.
        /// </summary>
        /// <seealso cref="OnGetItemTypeName"/>
        public string ItemTypeName
        {
            get => _itemTypeName ??= OnGetItemTypeName();
            set => _itemTypeName = value;
        }

        /// <summary>
        ///   Gets the name of the type of items stored by this repository (eg. "Asset", "Order" or "Customer"). 
        /// </summary>
        /// <see cref="ItemTypeName"/>
        protected virtual string OnGetItemTypeName() => typeof(T).Name;

        /// <summary>
        ///   Seeds the <see cref="Repository{T}"/> with a collection of items.
        /// </summary>
        public void Seed(IEnumerable<T> items) => _items = items
            .Where(i => !string.IsNullOrWhiteSpace(i.Id))
            .ToDictionary(i => i.Id!);

        protected bool IsInitializing { get; private set; }

        public virtual void BeginInitializing()
        {
            IsInitializing = true;
        }
        
        public virtual void EndInitializing()
        {
            IsInitializing = false;
        }

        /// <summary>
        ///   Gets a minimum delay (milliseconds) to simulate slow/long running operations. 
        /// </summary>
        public uint DelayMin
        {
            get => _delayMin;
            set
            {
                _delayMin = value;
                if (_delayMin > _delayMax)
                {
                    swap(ref _delayMin, ref _delayMax);
                }
            }
        }

        /// <summary>
        ///   Gets a maximum delay (milliseconds) to simulate slow/long running operations. 
        /// </summary>
        public uint DelayMax
        {
            get => _delayMax;
            set
            {
                _delayMax = value;
                if (_delayMax < _delayMin)
                {
                    swap(ref _delayMin, ref _delayMax);
                }
            }
        }

        static void swap(ref uint left, ref uint right) => (right, left) = (left, right);

        /// <summary>
        ///   Examines the repository and returns a value indicating whether a specific
        ///   item is currently contained within.
        /// </summary>
        /// <param name="id">
        ///   The item's identity.
        /// </param>
        /// <returns>
        ///   <c>true</c> if the item is currently recognized by the repository; otherwise <c>false</c>.
        /// </returns>
        public virtual Task<bool> ContainsAsync(string id) => Task.FromResult(_items.ContainsKey(id));

        /// <summary>
        ///   Injects a new item into the repository. 
        /// </summary>
        /// <param name="item">
        ///   The new item.
        /// </param>
        /// <param name="cancellation">
        ///   Allows cancelling the operation.
        /// </param>
        /// <returns>
        ///   An <see cref="Outcome{T}"/> to indicate success/failure and, on success, also carry
        ///   the new item's identity (<see cref="string"/>) or, on failure, an <see cref="Exception"/>.
        /// </returns>
        public virtual async Task<Outcome<string>> CreateAsync(T item, CancellationToken? cancellation = null)
        {
            await SimulateSlowRepositoryAsync(cancellation);

            var newItemOutcome = await OnMakeNewItemAsync(item);
            if (!newItemOutcome)
                return Outcome<string>.Fail(newItemOutcome.Exception);

            var newItem = newItemOutcome.Value!;
            var newId = newItem.Id!;
            if (string.IsNullOrEmpty(item.Id) && _items.ContainsKey(newId))
                return Outcome<string>.Fail(
                    new IdentityConflictException(item.Id, 
                        $"{ItemTypeName} with identity '{item.Id}' was already posted"));

            _items.Add(newId, newItem);
            return Outcome<string>.Success(newId);
        }

        protected abstract Task<Outcome<T>> OnMakeNewItemAsync(T source);

        /// <summary>
        ///   Reads one or more items from the repository.
        /// </summary>
        /// <param name="ids">
        ///   (optional)<br/>
        ///   One or more item identities.
        /// </param>
        /// <param name="failOnMissing">
        ///   (optional; default=<c>true</c>)<br/>
        ///   Specifies whether to treat a situation where one or more items could not be fond as a 'failure'.
        /// </param>
        /// <param name="cancellation">
        ///   Allows cancelling the operation.
        /// </param>
        /// <returns>
        ///   An <see cref="EnumOutcome{T}"/> to indicate success/failure and, on success, also carry
        ///   the requested items or, on failure, an <see cref="Exception"/>.
        /// </returns>
        public virtual async Task<EnumOutcome<T>> ReadAsync(
            string[]? ids = null,
            bool failOnMissing = true,
            CancellationToken? cancellation = null)
        {
            await SimulateSlowRepositoryAsync(cancellation);

            if (!ids?.Any() ?? true)
                return EnumOutcome<T>.Success(_items.Values.ToArray());

            var result = new List<T>();
            foreach (var id in ids!)
            {
                if (_items.TryGetValue(id, out var item))
                {
                    result.Add(item);
                    continue;
                }
                if (failOnMissing)
                    return EnumOutcome<T>.Fail(ServerException.NotFound($"{ItemTypeName} with id '{id}' was not found"));
            }

            return result.Count == 0
                ? EnumOutcome<T>.Fail()
                : EnumOutcome<T>.Success(result.ToArray());
            
        }
        
        protected virtual Task<T[]> ReadWhereAsync(Func<T, bool> criteria, CancellationToken? cancellation = null)
        {
            var ct = cancellation ?? CancellationToken.None;
            return Task.Run(() =>
            {
                var items = _items.Values.ToArray();
                var result = new List<T>();
                for (var i = 0; i < items.Length; i++)
                {
                    ct.ThrowIfCancellationRequested();
                    var item = items[i];
                    if (criteria(item))
                        result.Add(item);
                }

                return Task.FromResult(result.ToArray());
            }, ct);
            
        }

        public virtual async Task<Outcome<string>> CreateOrReplaceAsync(T item, CancellationToken? cancellation = null)
        {
            await SimulateSlowRepositoryAsync(cancellation);

            if (string.IsNullOrWhiteSpace(item.Id))
                return await CreateAsync(item);
            
            if (_items.TryGetValue(item.Id, out _))
                return await ReplaceAsync(item); 
                
            _items.Add(item.Id, item);
            return Outcome<string>.Success(item.Id);
            
        }

        /// <summary>
        ///   Replaces an existing item with a new one.
        /// </summary>
        /// <param name="item">
        ///   The new item.
        /// </param>
        /// <param name="cancellation">
        ///   Allows cancelling the operation.
        /// </param>
        /// <returns>
        ///   An <see cref="Outcome{T}"/> to indicate success/failure and, on success, also carry
        ///   the new (replacing) item's id (<see cref="string"/>) or, on failure, an <see cref="Exception"/>.
        /// </returns>
        public virtual async Task<Outcome<string>> ReplaceAsync(T item, CancellationToken? cancellation = null)
        {
            await SimulateSlowRepositoryAsync(cancellation);
            
            if (string.IsNullOrWhiteSpace(item.Id))
                return Outcome<string>.Fail(
                    ServerException.BadRequest($"Item must contain a valid identity ('{nameof(Model.Id)}')'"));
            
            if (!_items.TryGetValue(item.Id, out var existing))
                return Outcome<string>.Fail(
                    new ArgumentOutOfRangeException(
                        nameof(item), 
                        $"Repository does not contain asset '{item.Id}'"));

            _items[item.Id] = item;
            return Outcome<string>.Success(existing.Id!);
        }

        /// <summary>
        ///   Updates an existing item from a source item.
        /// </summary>
        /// <param name="item">
        ///   The source item.
        /// </param>
        /// <param name="cancellation">
        ///   Allows cancelling the operation.
        /// </param>
        /// <returns>
        ///   An <see cref="Outcome{T}"/> to indicate success/failure and, on success, also carry
        ///   the updated item's id (<see cref="string"/>) or, on failure, an <see cref="Exception"/>.
        /// </returns>
        public virtual async Task<Outcome<string>> UpdateAsync(T item, CancellationToken? cancellation = null)
        {
            await SimulateSlowRepositoryAsync(cancellation);
            
            if (string.IsNullOrWhiteSpace(item.Id))
                return Outcome<string>.Fail(
                    ServerException.BadRequest($"Item must contain a valid identity ({nameof(Model.Id)})'"));
            
            if (!_items.TryGetValue(item.Id, out var existing))
                return Outcome<string>.Fail(
                    new ArgumentOutOfRangeException(
                        nameof(item), 
                        $"Repository does not contain asset '{item.Id}'"));

            await OnUpdateItemAsync(existing, item);
            return Outcome<string>.Success(existing.Id!);
        }

        /// <summary>
        ///   Called from <see cref="UpdateAsync"/> to patch a target item wth values from a source item.
        /// </summary>
        /// <param name="target">
        ///   The item to be updated.
        /// </param>
        /// <param name="source">
        ///   An item to read new values from.
        /// </param>
        protected abstract Task OnUpdateItemAsync(T target, T source);

        /// <summary>
        ///   Removes an item from the repository.
        /// </summary>
        /// <param name="id">
        ///   The identity of the item to be removed.
        /// </param>
        /// <param name="cancellation">
        ///   Allows cancelling the operation.
        /// </param>
        /// <returns>
        ///   An <see cref="Outcome{T}"/> to indicate success/failure and, on success, also carry
        ///   the identity (<see cref="string"/>) of the removed item or, on failure, an <see cref="Exception"/>.
        /// </returns>
        public virtual async Task<Outcome<string>> DeleteAsync(string id, CancellationToken? cancellation = null)
        {
            await SimulateSlowRepositoryAsync(cancellation);
            
            if (string.IsNullOrWhiteSpace(id))
                return Outcome<string>.Fail(ServerException.BadRequest("Expected an item id"));
            
            if (!await ContainsAsync(id))
                return Outcome<string>.Fail(ServerException.NotFound($"Could not remove {ItemTypeName} '{id}'. Item was not found"));

            _items.Remove(id);
            return Outcome<string>.Success(id);
        }
        
        /// <summary>
        ///   Simulates a long running operation.
        /// </summary>
        /// <param name="cancellation">
        ///   Allows cancelling the operation.
        /// </param>
        protected Task SimulateSlowRepositoryAsync(CancellationToken? cancellation)
        {
            if (IsInitializing || _delayMax == 0)
                return Task.CompletedTask;

            var c = cancellation ?? CancellationToken.None;
            return Task.Run(async () =>
            {
                var rnd = new Random();
                var delay = rnd.Next((int)_delayMin, (int)_delayMax);
                var expire = DateTime.Now.AddMilliseconds(delay);
                while (DateTime.Now < expire)
                {
                    try
                    {
                        await Task.Delay(100, c);
                    }
                    catch (TaskCanceledException)
                    {
                        Logger.Information("Breaking out of Repository delay");
                        throw;
                    }
                }
            }, c);
        }
        
        public Repository(ILogger? logger)
        {
            Logger = logger;
            _items = new Dictionary<string, T>();
        }
    }
}