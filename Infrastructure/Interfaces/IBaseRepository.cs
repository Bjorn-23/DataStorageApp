using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Interfaces;

public interface IBaseRepository<TEntity, TContext> where TEntity : class where TContext : DbContext
{
    /// <summary>
    /// Creates a new entry in database.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns>TEntity</returns>
    TEntity Create(TEntity entity);

 

    /// <summary>
    /// Gets all existing entrys from database.
    /// </summary>
    /// <returns>List of TEntity</returns>
    IEnumerable<TEntity> GetAll();

    /// <summary>
    /// Gets all entries from database matching predicate/expression.
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns>List of TEntity</returns>
    IEnumerable<TEntity> GetAllWithPredicate(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// Gets on entry from database matching predicate/expression. 
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns>TEntity</returns>
    public TEntity GetOne(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// Updates a TEntity in the database
    /// </summary>
    /// <param name="predicate">any paramater of a property of TEntity that finds a match in database, search with unique property paramaters for best result.</param>
    /// <param name="entity">The updated TEntity details.</param>
    /// <returns>The updated TEntity if successful, else null.</returns>
    TEntity Update(Expression<Func<TEntity, bool>> predicate, TEntity entity);

    /// <summary>
    /// Updates a TEntity in the database.
    /// </summary>
    /// <param name="entity">The updated TEntity details, the Id will be the same.</param>
    /// <returns>TEntity</returns>
    TEntity Update(TEntity existingEntity, TEntity updatedEntity);

    /// <summary>
    /// Removes TEntity from the database.
    /// </summary>
    /// <param name="predicate">any paramater of a property of TEntity that finds a match in database, search with unique property paramaters for best result.</param>
    /// <returns>True if Deleted, else false.</returns>
    bool Delete(Expression<Func<TEntity, bool>> predicate);
    
    /// <summary>
    /// Removes TEntity from the database.
    /// </summary>
    /// <param name="predicate">any paramater of a property of TEntity that finds a match in database, search with unique property paramaters for best result.</param>
    /// <returns>True if Deleted, else false.</returns>
    bool Delete(TEntity entity);

    /// <summary>
    /// Checks if something exists in database.
    /// </summary>
    /// <param name="predicate">any paramater of a property of TEntity that finds a match in database, search with unique property paramaters for best result.</param>
    /// <returns>True if Exists, else false.</returns>
    bool Exists(Expression<Func<TEntity, bool>> predicate);
}