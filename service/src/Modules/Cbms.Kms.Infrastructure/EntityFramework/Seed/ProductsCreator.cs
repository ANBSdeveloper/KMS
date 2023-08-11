namespace Cbms.AspNetCore.Test.Seed
{
    public class ProductsCreator
    {
        private readonly HostDbContext _context;

        public ProductsCreator(HostDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            for (int i = 0; i < 10; i++)
            {
                Product product = new Product()
                {
                    Name = $"Product {i}",
                    Description = $"Product {i}"
                };
                _context.Products.Add(product);
            }

            _context.SaveChanges();
        }
    }
}
