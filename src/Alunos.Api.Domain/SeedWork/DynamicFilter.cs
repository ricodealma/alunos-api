using System.Linq.Expressions;

namespace Alunos.Api.Domain.SeedWork;

public static class DynamicFilter
{
    public static Expression<Func<T, bool>>? GenerateFilter<T>(List<Expression<Func<T, bool>>> filters)
    {
        if (filters.Count == 0)
            return null;

        Expression<Func<T, bool>> combinedFilter = filters[0];

        for (int i = 1; i < filters.Count; i++)
        {
            combinedFilter = CombineWith(combinedFilter, filters[i], Expression.AndAlso);
        }

        return combinedFilter;
    }

    private static Expression<Func<T, bool>> CombineWith<T>(
        Expression<Func<T, bool>> first,
        Expression<Func<T, bool>> second,
        Func<Expression, Expression, BinaryExpression> merge)
    {
        var parameter = Expression.Parameter(typeof(T));

        var leftVisitor = new ReplaceExpressionVisitor(first.Parameters[0], parameter);
        var left = leftVisitor.Visit(first.Body);

        var rightVisitor = new ReplaceExpressionVisitor(second.Parameters[0], parameter);
        var right = rightVisitor.Visit(second.Body);

        return Expression.Lambda<Func<T, bool>>(merge(left!, right!), parameter);
    }

    private class ReplaceExpressionVisitor(Expression oldValue, Expression newValue) : ExpressionVisitor
    {
        private readonly Expression _oldValue = oldValue;
        private readonly Expression _newValue = newValue;

        public override Expression? Visit(Expression? node)
        {
            return node == _oldValue ? _newValue : base.Visit(node);
        }
    }
}
