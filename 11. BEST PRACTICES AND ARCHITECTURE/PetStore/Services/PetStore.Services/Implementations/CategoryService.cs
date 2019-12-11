using Petstore.Data;
using Petstore.Data.Models;
using PetStore.Services.Models.Category;
using System.Collections.Generic;
using System.Linq;

namespace PetStore.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly PetstoreDbContext data;

        public CategoryService(PetstoreDbContext data)
        {
            this.data = data;
        }

        public IEnumerable<AllCategoriesServiceModel> All()
        {
            return this.data.Categories
                .Select(c => new AllCategoriesServiceModel
                {
                    Name = c.Name,
                    Id = c.Id,
                    Description = c.Description
                })
                .ToArray();
        }

        public void Create(CreateCategoryServiceModel model)
        {
            var category = new Category
            {
                Name = model.Name,
                Description = model.Description
            };

            this.data.Categories.Add(category);

            this.data.SaveChanges();
        }

        public void Edit(EditCategoryServiceModel model)
        {
            var category = this.data.Categories.Find(model.Id);

            category.Name = model.Name;

            category.Description = model.Description;

            this.data.SaveChanges();
        }

        public bool Exists(int categoryId)
        {
            return this.data.Categories.Any(c => c.Id == categoryId);
        }

        public DetailsCategoryServiceModel GetById(int id)
        {
            var category = this.data.Categories.Find(id);

            var dcsm = new DetailsCategoryServiceModel
            {
                Id = category?.Id,
                Name = category?.Name,
                Description = category?.Description
            };

            return dcsm;
        }

        public bool Remove(int id)
        {
            var category = this.data.Categories.Find(id);

            if (category == null)
            {
                return false;
            }

            this.data.Categories.Remove(category);

            this.data.SaveChanges();

            return true;
        }
    }
}
