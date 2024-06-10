using Microsoft.EntityFrameworkCore;
using ProductConsoleAPI.Business;
using ProductConsoleAPI.Business.Contracts;
using ProductConsoleAPI.Data.Models;
using ProductConsoleAPI.DataAccess;
using System.ComponentModel.DataAnnotations;

namespace ProductConsoleAPI.IntegrationTests.NUnit
{
    public  class IntegrationTests
    {
        private TestProductsDbContext dbContext;
        private IProductsManager productsManager;

        [SetUp]
        public void SetUp()
        {
            this.dbContext = new TestProductsDbContext();
            this.productsManager = new ProductsManager(new ProductsRepository(this.dbContext));
        }


        [TearDown]
        public void TearDown()
        {
            this.dbContext.Database.EnsureDeleted();
            this.dbContext.Dispose();
        }


        //positive test
        [Test]
        public async Task AddProductAsync_ShouldAddNewProduct()
        {
            var newProduct = new Product()
            {
                OriginCountry = "Bulgaria",
                ProductName = "TestProduct",
                ProductCode = "AB12C",
                Price = 1.25m,
                Quantity = 100,
                Description = "Anything for description"
            };

            await productsManager.AddAsync(newProduct);

            var dbProduct = await this.dbContext.Products.FirstOrDefaultAsync(p => p.ProductCode == newProduct.ProductCode);
            Assert.Multiple(() =>
            {
                Assert.That(dbProduct, Is.Not.Null);
                Assert.That(dbProduct.ProductName, Is.EqualTo(newProduct.ProductName));
                Assert.That(dbProduct.Description, Is.EqualTo(newProduct.Description));
                Assert.That(dbProduct.Price, Is.EqualTo(newProduct.Price));
                Assert.That(dbProduct.Quantity, Is.EqualTo(newProduct.Quantity));
                Assert.That(dbProduct.OriginCountry, Is.EqualTo(newProduct.OriginCountry));
                Assert.That(dbProduct.ProductCode, Is.EqualTo(newProduct.ProductCode));
            });         
        }

        //Negative test
        [Test]
        public async Task AddProductAsync_TryToAddProductWithInvalidCredentials_ShouldThrowException()
        {
            var newProduct = new Product()
            {
                OriginCountry = "Bulgaria",
                ProductName = "TestProduct",
                ProductCode = "AB12C",
                Price = -1m,
                Quantity = 100,
                Description = "Anything for description"
            };

            var ex = Assert.ThrowsAsync<ValidationException>(async () => await productsManager.AddAsync(newProduct));
            var actual = await dbContext.Products.FirstOrDefaultAsync(c => c.ProductCode == newProduct.ProductCode);

            Assert.IsNull(actual);
            Assert.That(ex?.Message, Is.EqualTo("Invalid product!"));

        }

        [Test]
        public async Task DeleteProductAsync_WithValidProductCode_ShouldRemoveProductFromDb()
        {
            // Arrange
            var newPoduct = new Product()
            {
                OriginCountry = "Bulgaria",
                ProductName = "TestProduct",
                ProductCode = "AB12C",
                Price = 1.25m,
                Quantity = 100,
                Description = "Anything for description"
            };
            await productsManager.AddAsync(newPoduct);

            // Act
            await productsManager.DeleteAsync(newPoduct.ProductCode);

            // Assert
            var productInTheDB = await dbContext.Products.FirstOrDefaultAsync(x => x.ProductCode == newPoduct.ProductCode);
            Assert.IsNull(productInTheDB);
        }

        [Test]
        public async Task DeleteProductAsync_TryToDeleteWithNullOrWhiteSpaceProductCode_ShouldThrowException()
        {
            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(() => productsManager.DeleteAsync(" "));
            Assert.That(exception.Message, Is.EqualTo("Product code cannot be empty."));
        }

        [Test]
        public async Task GetAllAsync_WhenProductsExist_ShouldReturnAllProducts()
        {
            var expectedProducts = new List<Product>
            {
                new Product 
                { 
                    OriginCountry = "Bulgaria",
                    ProductName = "Product1",
                    ProductCode = "AB12C",
                    Price = 10m,
                    Quantity = 100,
                    Description = "Description 1" 
                },

                new Product 

                { OriginCountry = "USA",
                    ProductName = "Product2",
                    ProductCode = "AB12D",
                    Price = 20m,
                    Quantity = 200,
                    Description = "Description 2"
                }
            };
            await this.dbContext.Products.AddRangeAsync(expectedProducts);
            await this.dbContext.SaveChangesAsync();

            var actualProducts = await productsManager.GetAllAsync();

            CollectionAssert.AreEquivalent(expectedProducts, actualProducts);
        }

        [Test]
        public async Task GetAllAsync_WhenNoProductsExist_ShouldThrowKeyNotFoundException()
        {
            // Arrange

            // Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await productsManager.GetAllAsync());
            Assert.That(ex?.Message, Is.EqualTo("No product found."));
        }

        [Test]
        public async Task SearchByOriginCountry_WithExistingOriginCountry_ShouldReturnMatchingProducts()
        {
            var originCountry = "Bulgaria";
            var expectedProducts = new List<Product>
            {
                new Product 
                {
                    OriginCountry = "Bulgaria",
                    ProductName = "Product1",
                    ProductCode = "AB12C",
                    Price = 10m,
                    Quantity = 100,
                    Description = "Description 1"
                },

                new Product 

                {
                    OriginCountry = "Bulgaria",
                    ProductName = "Product2",
                    ProductCode = "AB12D",
                    Price = 20m,
                    Quantity = 200,
                    Description = "Description 2"
                }
            };
            await this.dbContext.Products.AddRangeAsync(expectedProducts);
            await this.dbContext.SaveChangesAsync();

            var actualProducts = await productsManager.SearchByOriginCountry(originCountry);

            CollectionAssert.AreEquivalent(expectedProducts, actualProducts);
        }

        [Test]
        public async Task SearchByOriginCountryAsync_WithNonExistingOriginCountry_ShouldThrowKeyNotFoundException()
        {
            var nonExistingOriginCountry = "NonExistingCountry";

            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await productsManager.SearchByOriginCountry(nonExistingOriginCountry));
            Assert.That(ex?.Message, Is.EqualTo("No product found with the given first name."));
        }

        [Test]
        public async Task GetSpecificAsync_WithValidProductCode_ShouldReturnProduct()
        {
            var product = new Product 
            {
                OriginCountry = "Bulgaria",
                ProductName = "TestProduct",
                ProductCode = "AB12C",
                Price = 10m,
                Quantity = 100,
                Description = "Description 1" 
            };
            await this.dbContext.Products.AddAsync(product);
            await this.dbContext.SaveChangesAsync();

            var retrievedProduct = await productsManager.GetSpecificAsync(product.ProductCode);

            Assert.NotNull(retrievedProduct);
            Assert.That(retrievedProduct, Is.EqualTo(product));
        }

        [Test]
        public async Task GetSpecificAsync_WithInvalidProductCode_ShouldThrowKeyNotFoundException()
        {
            var invalidProductCode = "InvalidCode";

            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await productsManager.GetSpecificAsync(invalidProductCode));
            Assert.That(ex?.Message, Is.EqualTo($"No product found with product code: {invalidProductCode}"));
        }

        [Test]
        public async Task UpdateAsync_WithValidProduct_ShouldUpdateProduct()
        {
            // Arrange
            var originalProduct = new Product()
            {
                OriginCountry = "Bulgaria",
                ProductName = "OriginalProduct",
                ProductCode = "AB12C",
                Price = 1.25m,
                Quantity = 100,
                Description = "Original Description"
            };

            await dbContext.Products.AddAsync(originalProduct);
            await dbContext.SaveChangesAsync();

            var updatedProduct = new Product()
            {
                OriginCountry = "USA",
                ProductName = "UpdatedProduct",
                ProductCode = "AB12D",
                Price = 2.50m,
                Quantity = 200,
                Description = "Updated Description"
            };

            // Act
            await productsManager.UpdateAsync(updatedProduct);

            var dbProduct = await dbContext.Products.FirstOrDefaultAsync(p => p.ProductCode == updatedProduct.ProductCode);

            // Assert
            Assert.NotNull(dbProduct);
            Assert.That(dbProduct.ProductName, Is.EqualTo(updatedProduct.ProductName));
            Assert.That(dbProduct.Description, Is.EqualTo(updatedProduct.Description));
            Assert.That(dbProduct.Price, Is.EqualTo(updatedProduct.Price));
            Assert.That(dbProduct.Quantity, Is.EqualTo(updatedProduct.Quantity));
            Assert.That(dbProduct.OriginCountry, Is.EqualTo(updatedProduct.OriginCountry));
            Assert.That(dbProduct.ProductCode, Is.EqualTo(updatedProduct.ProductCode));
        }

        [Test]
        public async Task UpdateAsync_WithInvalidProduct_ShouldThrowValidationException()
        {
            // Arrange
            var invalidProduct = new Product()
            {
                OriginCountry = "Bulgaria",
                ProductName = null,
                ProductCode = "AB12C",
                Price = 1.25m,
                Quantity = 100,
                Description = "Anything for description"
            };

            // Act & Assert
            var ex = Assert.ThrowsAsync<ValidationException>(async () => await productsManager.UpdateAsync(invalidProduct));
            Assert.That(ex?.Message, Is.EqualTo("Invalid product!"));
        }
    }
}
