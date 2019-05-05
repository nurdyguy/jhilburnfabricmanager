using JHilburnFabricManager.Models;
using JHilburnFabricManager.Services.Models;
using System.Threading.Tasks;

namespace JHilburnFabricManager.Services.Contracts
{
    public interface IFabricDataService
    {
        //[GET] /fabrics/:id - Get a fabric by id
        Task<Fabric> Get(int fabricId);

        //[GET] /fabrics - Get a list of all fabrics -- paged response
        Task<ApiPagedResponse<Fabric>> Get(int page, int perPage, string search, string sortBy, string sortDir);
            
        //[POST] /fabrics - Create a new fabric
        Task<Fabric> Create(Fabric fabric);
        
        //[PUT] /fabrics/:id - Update fabric record
        Task<Fabric> Update(Fabric fabric);

        //[PATCH] /fabrics/:id - Update specifc fields of a fabric record
        //------

        //[DELETE] /fabrics/:id - Delete a fabric
        Task<bool> Delete(int fabricId);

    }
}
