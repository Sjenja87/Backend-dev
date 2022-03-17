#nullable disable
using Microsoft.EntityFrameworkCore;
using ModelManagement.Data;
using ModelManagement.Models;

namespace ModelManagement.Controllers
{
    public class JobConnector : IConnector
    {
      #region Constuctor
      
        public JobConnector(ModelDb context)
        {
          Context = context;
        }
      
      #endregion
      
      #region Properties

        public ModelDb Context { get; }
      
      #endregion
        
      #region Methods

        public async void RemoveElementFromNavigator(long jobId, long modelId)
        { 
          Job? job = await Context.Jobs.FindAsync(jobId);

          if (JobIsNull(job)) return;
          
          job!.Models = await RemoveModelAsync(job, modelId); 
          await Context.SaveChangesAsync();
        }

        public async void AddElementToNavigator(long jobId, long modelId)
        {
          Job? job = await Context.Jobs.FindAsync(jobId);

          if (JobIsNull(job)) return;
          
          job.Models = await AddModelAsync(job, modelId);
          await Context.SaveChangesAsync();
        }

      #endregion
      
      #region private Methods
        private async Task<ICollection<Model>> AddModelAsync(Job job, long modelId)
        {
          Model model = await Context.Models.FindAsync(modelId);
          List<Model> models = await Context.Entry(job)
                                      .Collection(j => j.Models)
                                      .Query()
                                      .ToListAsync();

          if (ModelIsInList(model, models))
          {
            return models;
          }
          
          models.Add(model);
          return models;
        }

        private async Task<ICollection<Model>> RemoveModelAsync(Job job, long modelId) 
        { 
          List<Model> updatedModelsList = await Context.Entry(job)
                                                .Collection(j => j.Models)
                                                .Query()
                                                .ToListAsync();
          return updatedModelsList
                  .Where(m => m.ModelId != modelId)
                  .ToList();
        } 
        
        private static bool JobIsNull(Job? job) 
          => job == null;

        private static bool ModelIsNull(Model? model)
          => model == null;

        private static bool ModelIsInList(Model model, ICollection<Model> models)
        {
          return models.Contains(model);
        }
        #endregion

    }

}
