// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Arc.Validation;
using Cratis.Monads;
using Library.Inventory.Adding;

namespace Library.Lending.Reserving;

public record Book(int Available);

public class BookProjection : IProjectionFor<Book>
{
    public void Define(IProjectionBuilderFor<Book> builder) => builder
        .Passive()
        .From<BookAddedToInventory>(e => e.Increment(book => book.Available))
        .From<BookReserved>(e => e.Decrement(book => book.Available));
}

[Command]
public record ReserveBook(ISBN Isbn, MemberId MemberId)
{
    public Result<BookReserved, ValidationResult> Handle(Book book)
    {
        if (book.Available <= 0)
        {
            return ValidationResult.Error($" No available copies to reserve for book {Isbn}");
        }

        return new BookReserved(Isbn, MemberId);
    }
}

[EventType]
public record BookReserved(ISBN Isbn, MemberId MemberId);
