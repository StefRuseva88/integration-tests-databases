using ContactsConsoleAPI.Business;
using ContactsConsoleAPI.Business.Contracts;
using ContactsConsoleAPI.Data.Models;
using ContactsConsoleAPI.DataAccess;
using ContactsConsoleAPI.DataAccess.Contrackts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsConsoleAPI.IntegrationTests.NUnit
{
    public class IntegrationTests
    {
        private TestContactDbContext dbContext;
        private IContactManager contactManager;

        [SetUp]
        public void SetUp()
        {
            this.dbContext = new TestContactDbContext();
            this.contactManager = new ContactManager(new ContactRepository(this.dbContext));
        }


        [TearDown]
        public void TearDown()
        {
            this.dbContext.Database.EnsureDeleted();
            this.dbContext.Dispose();
        }


        //positive test
        [Test]
        public async Task AddContactAsync_ShouldAddNewContact()
        {
            var newContact = new Contact()
            {
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Address = "Anything for testing address",
                Contact_ULID = "1ABC23456HH", //must be minimum 10 symbols - numbers or Upper case letters
                Email = "test@gmail.com",
                Gender = "Male",
                Phone = "0889933779"
            };

            await contactManager.AddAsync(newContact);

            var dbContact = await dbContext.Contacts.FirstOrDefaultAsync(c => c.Contact_ULID == newContact.Contact_ULID);
            Assert.Multiple(() =>
            { 
            Assert.NotNull(dbContact);
            Assert.That(dbContact.FirstName, Is.EqualTo(newContact.FirstName));
            Assert.That(dbContact.LastName, Is.EqualTo(newContact.LastName));
            Assert.That(dbContact.Phone, Is.EqualTo(newContact.Phone));
            Assert.That(dbContact.Email, Is.EqualTo(newContact.Email));
            Assert.That(dbContact.Address, Is.EqualTo(newContact.Address));
            Assert.That(dbContact.Contact_ULID, Is.EqualTo(newContact.Contact_ULID));
            });
        }

        //Negative test
        [Test]
        public async Task AddContactAsync_TryToAddContactWithInvalidCredentials_ShouldThrowException()
        {
            var newContact = new Contact()
            {
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Address = "Anything for testing address",
                Contact_ULID = "1ABC23456HH", //must be minimum 10 symbols - numbers or Upper case letters
                Email = "invalid_Mail", //invalid email
                Gender = "Male",
                Phone = "0889933779"
            };

            var ex = Assert.ThrowsAsync<ValidationException>(async () => await contactManager.AddAsync(newContact));
            var actual = await dbContext.Contacts.FirstOrDefaultAsync(c => c.Contact_ULID == newContact.Contact_ULID);

            Assert.IsNull(actual);
            Assert.That(ex?.Message, Is.EqualTo("Invalid contact!"));

        }

        [Test]
        public async Task DeleteContactAsync_WithValidULID_ShouldRemoveContactFromDb()
        {
            // Arrange
            var newContact = new Contact()
            {
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Address = "Anything for testing address",
                Contact_ULID = "1ABC23456HH",
                Email = "test@gmail.com",
                Gender = "Male",
                Phone = "0889933779"
            };

            await dbContext.Contacts.AddAsync(newContact);
            await dbContext.SaveChangesAsync();

            // Act
            await contactManager.DeleteAsync(newContact.Contact_ULID);

            // Assert
            var dbContact = await dbContext.Contacts.FirstOrDefaultAsync(c => c.Contact_ULID == newContact.Contact_ULID);
            Assert.That(dbContact, Is.Null);
        }

        [Test]
        public async Task DeleteContactAsync_TryToDeleteWithNullOrWhiteSpaceULID_ShouldThrowException()
        {
            // Arrange

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await contactManager.DeleteAsync(null));
            Assert.That(ex?.Message, Is.EqualTo("ULID cannot be empty."));
        }

        [Test]
        public async Task GetAllAsync_WhenContactsExist_ShouldReturnAllContacts()
        {
            // Arrange
            var newContact1 = new Contact()
            {
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Address = "Anything for testing address",
                Contact_ULID = "1ABC23456HH",
                Email = "test@gmail.com",
                Gender = "Male",
                Phone = "0889933779"
            };

            var newContact2 = new Contact()
            {
                FirstName = "AnotherFirstName",
                LastName = "AnotherLastName",
                Address = "Another address for testing",
                Contact_ULID = "2DEF34567II",
                Email = "another@test.com",
                Gender = "Female",
                Phone = "0999888666"
            };

            await dbContext.Contacts.AddRangeAsync(newContact1, newContact2);
            await dbContext.SaveChangesAsync();

            // Act
            var result = await contactManager.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetAllAsync_WhenNoContactsExist_ShouldThrowKeyNotFoundException()
        {
            // Arrange

            // Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await contactManager.GetAllAsync());
            Assert.That(ex?.Message, Is.EqualTo("No contact found."));
        }

        [Test]
        public async Task SearchByFirstNameAsync_WithExistingFirstName_ShouldReturnMatchingContacts()
        {
            // Arrange
            var newContact1 = new Contact()
            {
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Address = "Anything for testing address",
                Contact_ULID = "1ABC23456HH",
                Email = "test@gmail.com",
                Gender = "Male",
                Phone = "0889933779"
            };

            var newContact2 = new Contact()

            {
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Address = "Anything for testing address",
                Contact_ULID = "2DEF34567II",
                Email = "test2@gmail.com",
                Gender = "Female",
                Phone = "0889933788"
            };

            await dbContext.Contacts.AddRangeAsync(newContact1, newContact2);
            await dbContext.SaveChangesAsync();

            // Act
            var result = await contactManager.SearchByFirstNameAsync("TestFirstName");

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task SearchByFirstNameAsync_WithNonExistingFirstName_ShouldThrowKeyNotFoundException()
        {
            // Arrange

            // Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await contactManager.SearchByFirstNameAsync("NonExistingFirstName"));
            Assert.That(ex?.Message, Is.EqualTo("No contact found with the given first name."));
        }

        [Test]
        public async Task SearchByLastNameAsync_WithExistingLastName_ShouldReturnMatchingContacts()
        {
            // Arrange
            var newContact1 = new Contact()
            {
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Address = "Anything for testing address",
                Contact_ULID = "1ABC23456HH",
                Email = "test@gmail.com",
                Gender = "Male",
                Phone = "0889933779"
            };

            var newContact2 = new Contact()
            {
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Address = "Anything for testing address",
                Contact_ULID = "2DEF34567II",
                Email = "test2@gmail.com",
                Gender = "Female",
                Phone = "0889933788"
            };

            await dbContext.Contacts.AddRangeAsync(newContact1, newContact2);
            await dbContext.SaveChangesAsync();

            // Act
            var result = await contactManager.SearchByLastNameAsync("TestLastName");

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task SearchByLastNameAsync_WithNonExistingLastName_ShouldThrowKeyNotFoundException()
        {
            // Arrange

            // Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await contactManager.SearchByLastNameAsync("NonExistingLastName"));
            Assert.That(ex?.Message, Is.EqualTo("No contact found with the given last name."));
        }

        [Test]
        public async Task GetSpecificAsync_WithValidULID_ShouldReturnContact()
        {
            // Arrange
            var newContact = new Contact()
            {
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Address = "Anything for testing address",
                Contact_ULID = "1ABC23456HH",
                Email = "test@gmail.com",
                Gender = "Male",
                Phone = "0889933779"
            };

            await dbContext.Contacts.AddAsync(newContact);
            await dbContext.SaveChangesAsync();

            // Act
            var result = await contactManager.GetSpecificAsync(newContact.Contact_ULID);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Contact_ULID, Is.EqualTo(newContact.Contact_ULID));
        }

        [Test]
        public async Task GetSpecificAsync_WithInvalidULID_ShouldThrowKeyNotFoundException()
        {
            // Arrange

            // Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await contactManager.GetSpecificAsync("invalidULID"));
            Assert.That(ex?.Message, Is.EqualTo("No contact found with ULID: invalidULID"));
        }

        [Test]
        public async Task UpdateAsync_WithValidContact_ShouldUpdateContact()
        {
            // Arrange
            var newContact = new Contact()
            {
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Address = "Anything for testing address",
                Contact_ULID = "1ABC23456HH",
                Email = "test@gmail.com",
                Gender = "Male",
                Phone = "0889933779"
            };

            await dbContext.Contacts.AddAsync(newContact);
            await dbContext.SaveChangesAsync();

            // Update the contact
            newContact.FirstName = "UpdatedFirstName";
            newContact.LastName = "UpdatedLastName";
            newContact.Phone = "0999888777";

            // Act
            await contactManager.UpdateAsync(newContact);

            // Assert
            var updatedContact = await dbContext.Contacts.FirstOrDefaultAsync(c => c.Contact_ULID == newContact.Contact_ULID);
            Assert.NotNull(updatedContact);
            Assert.That(updatedContact.FirstName, Is.EqualTo(newContact.FirstName));
            Assert.That(updatedContact.LastName, Is.EqualTo(newContact.LastName));
            Assert.That(updatedContact.Phone, Is.EqualTo(newContact.Phone));
        }

        [Test]
        public async Task UpdateAsync_WithInvalidContact_ShouldThrowValidationException()
        {
            // Arrange
            var invalidContact = new Contact(); 

            // Act & Assert
            var ex = Assert.ThrowsAsync<ValidationException>(async () => await contactManager.UpdateAsync(invalidContact));
            Assert.That(ex?.Message, Is.EqualTo("Invalid contact!"));
        }
    }
}
