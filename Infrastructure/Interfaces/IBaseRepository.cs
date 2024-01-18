using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Interfaces;

//public interface IBaseRepository<TEntity, TContext> where TEntity : class where TContext : DbContext
//{
//    TEntity Create(TEntity entity);
//    bool Delete(Expression<Func<TEntity, bool>> predicate);
//    bool Exists(Expression<Func<TEntity, bool>> predicate);
//    IEnumerable<TEntity> GetAll();
//    TEntity Update(Expression<Func<TEntity, bool>> predicate, TEntity entity);
//    TEntity Update(TEntity entity);
//}

public interface IBaseRepository<TEntity, TContext> where TEntity : class where TContext : DbContext
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
    /// <returns>IEnumerable list of TEntities.</returns>
    IEnumerable<TEntity> GetAll();

    IEnumerable<TEntity> GetAllWithPredicate(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// Gets on TEntity from database.
    /// </summary>
    /// <param name="entity">A TEntity.</param>
    /// <returns>A TEntity if a match is found, else null.</returns>
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
    /// <returns>A TEntity</returns>
    TEntity Update(TEntity entity);
}