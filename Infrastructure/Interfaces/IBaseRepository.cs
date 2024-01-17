using System.Linq.Expressions;

namespace Infrastructure.Interfaces;

public interface IBaseRepository<TEntity> where TEntity : class
{
    /// <summary>
    /// Creates a new TEntity in database.
    /// </summary>
    /// <param name="entity">The entity that will be created.</param>
    /// <returns>The created Entity.</returns>
    TEntity Create(TEntity entity);

    /// <summary>
    /// Removes TEntity from the database.
    /// </summary>
    /// <param name="predicate">any paramater of a property of TEntity that finds a match in database, search with unique property paramaters for best result.</param>
    /// <returns>True if Deleted, else false.</returns>
    bool Delete(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// Checks if something exists in database.
    /// </summary>
    /// <param name="predicate">any paramater of a property of TEntity that finds a match in database, search with unique property paramaters for best result.</param>
    /// <returns>True if Exists, else false.</returns>
    bool Exists(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// Returns all TEntities from database.
    /// </summary>
    /// <returns>IEnumerable list of TEntities</returns>
    IEnumerable<TEntity> GetAll();

    /// <summary>
    /// Updates a TEntity in the database
    /// </summary>
    /// <param name="predicate">any paramater of a property of TEntity that finds a match in database, search with unique property paramaters for best result.</param>
    /// <param name="entity">The updated TEntity details</param>
    /// <returns>The updated TEntity if successful, else null</returns>
    TEntity Update(Expression<Func<TEntity, bool>> predicate, TEntity entity);

    /// <summary>
    /// Updates a TEntity in the database
    /// </summary>
    /// <param name="entity">The updated TEntity details, the Id will be the same</param>
    /// <returns></returns>
    TEntity Update(TEntity entity);
}