using NikuAPI.Entities;

namespace NikuAPI.IRepository;

public interface IBoneRepository
{
    Task<Bone> GetBoneById(string nationalCode);
    Task<string> UpdateBone(string nationalCode);
}
