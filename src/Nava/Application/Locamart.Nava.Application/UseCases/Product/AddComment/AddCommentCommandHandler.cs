using CSharpFunctionalExtensions;
using Locamart.Dina;
using Locamart.Dina.Abstracts;
using Locamart.Dina.Infrastructure;
using Locamart.Nava.Application.Contracts.UseCases.Product.AddComment;
using Locamart.Nava.Domain.Entities.Comment;
using Locamart.Nava.Domain.Entities.Comment.Abstracts;
using Locamart.Nava.Domain.Entities.Comment.RequestModels;
using Locamart.Nava.Domain.Entities.Comment.ValueObjects;
using Locamart.Nava.Domain.Entities.Product.Abstracts;
using Locamart.Nava.Domain.Entities.Product.ValueObjects;

namespace Locamart.Nava.Application.UseCases.Product.AddComment;

public class AddCommentCommandHandler(IProductRepository productRepository, ICommentRepository commentRepository, IUnitOfWork unitOfWork) : ICommandHandler<AddCommentCommand, UnitResult<Error>>
{
    public async Task<UnitResult<Error>> Handle(AddCommentCommand request, CancellationToken cancellationToken)
    {
        var productId = ProductId.Create(request.ProductId);

        if (productId.IsFailure)
            return productId.Error;

        var parentId = request.ParentId.HasValue ? CommentId.Create(request.ParentId.Value).Value : null;

        var product = await productRepository.GetByIdAsync(productId.Value, cancellationToken);

        if (product is null || product.IsDeleted)
            return Error.Create("product_not_exists", "Provided product id does not exist");

        var comment = CommentEntity.Create(productId.Value, parentId, request.BodyMarkdown);

        if (comment.IsFailure)
            return comment.Error;


        var attachmentResults = request.Attachments?
                                    .Select(dto => CommentAttachmentEntity.Create(new AddCommentAttachmentRequest
                                    {
                                        CommentId = comment.Value.Id.Value,
                                        Url = dto.Url,
                                        Width = dto.Width,
                                        Height = dto.Height
                                    }))
                                ?? [];

        var firstFailure = attachmentResults.FirstOrDefault(r => r.IsFailure);

        if (firstFailure.Value is not null)
            return firstFailure.Error;

        IEnumerable<CommentAttachmentEntity> attachments = attachmentResults.Select(r => r.Value);

        comment.Value.AddAttachments(attachments);

        await commentRepository.AddAsync(comment.Value, cancellationToken);

        await unitOfWork.CommitAsync(cancellationToken);

        return UnitResult.Success<Error>();
    }
}

