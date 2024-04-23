using AutoMapper;
using bookStore.API.Data;
using bookStore.API.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookStore.API.Repository
{
    public class BookRepository : IBookRepository
    {
        
        private readonly BookStoreContext _context;
        private readonly IMapper _mapper;

        public BookRepository(BookStoreContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //GetAll Items Start
        public async Task<List<BookModel>> GetAllBooksAsync()
        {
            var records = await _context.Books.Select(x => new BookModel
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                
            }).ToListAsync();

            return records;
        }
        //GetAllItems End

        //GetOneItem start
        public async Task<BookModel> GetBookByIdAsync(int bookId)
        {
            //1st way to get Item it's need to write too many code
            //var book = await _context.Books.Where(x => x.Id == bookId).Select(x => new BookModel()
            //{
            //    Id = x.Id,
            //    Name = x.Name,
            //    Description = x.Description,
            //}).FirstOrDefaultAsync();
            //return book;

            //2nd way to get Item by Id using AutoMapper
            //here book is Books type
            var book = await _context.Books.FindAsync(bookId);
            return _mapper.Map<BookModel>(book); //it's convert Books type to BookModel type

        }//GetOneItem End

        //AddItem start
        public async Task<int> AddBookAsync(BookModel bookModel)
        {
            var book = new Books()
            {
                Name = bookModel.Name,
                Description = bookModel.Description,
            };
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return book.Id;
        }//addItem End

        //EditItem start for HttpPut
        public async Task UpdateBookAsync(int bookId, BookModel bookModel)
        {
            //update 1st way
            //var book = await _context.Books.FindAsync(bookId);
            //if(book != null)
            //{
            //    book.Name = bookModel.Name;
            //    book.Description = bookModel.Description;
            //    await _context.SaveChangesAsync();
            //}

            var book = new Books()
            {
                Id=bookId,
                Name = bookModel.Name,
                Description = bookModel.Description,
            };
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }//editItem end Httput

        //EditItem start for HttpPatch
        public async Task UpdateBookPatchAsync(int bookId, JsonPatchDocument bookModel)
        {
            var book = await _context.Books.FindAsync(bookId);
            if(book != null)
            {
                bookModel.ApplyTo(book);
                await _context.SaveChangesAsync();
            }
        }
        //EditItem end

        //DeleteItem start
        public async Task DeleteBookAsync(int bookId)
        {
            var book = new Books()
            {
                Id = bookId,
            };
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }//deleteItem end 
        

        





    }
}
