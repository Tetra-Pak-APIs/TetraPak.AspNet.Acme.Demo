using System;
using System.Threading;
using System.Threading.Tasks;
using TetraPak;

namespace demo.Acme
{
    /// <summary>
    ///   Classes implementing this interface can be used as a basic repository for a specific type of resources. 
    /// </summary>
    /// <typeparam name="T">
    ///   The resource type supported by the repository.
    /// </typeparam>
    public interface IRepository<T>
    {
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
        Task<bool> ContainsAsync(string id);
        
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
        Task<Outcome<string>> CreateAsync(
            T item, 
            CancellationToken? cancellation = null);

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
        Task<EnumOutcome<T>> ReadAsync(
            string[]? ids = null,
            bool failOnMissing = true,
            CancellationToken? cancellation = null);

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
        Task<Outcome<string>> ReplaceAsync(T item, CancellationToken? cancellation = null);

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
        Task<Outcome<string>> UpdateAsync(T item, CancellationToken? cancellation = null);

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
        Task<Outcome<string>> DeleteAsync(string id, CancellationToken? cancellation = null);
    }
}