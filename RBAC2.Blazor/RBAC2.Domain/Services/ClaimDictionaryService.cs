using Microsoft.EntityFrameworkCore;
using RBAC2.Database;
using RBAC2.Database.Entities;

public class ClaimDictionaryService : IClaimDictionaryService
{
    private readonly RbacDbContext _context;

    public ClaimDictionaryService(RbacDbContext context)
    {
        _context = context;
    }

    public async Task<ClaimDictionaryModel> CreateClaimAsync(ClaimDictionaryModel claimModel)
    {
        var claimEntity = new ClaimDictionary
        {
            ClaimType = claimModel.ClaimType,
            ClaimValue = claimModel.ClaimValue
        };

        _context.ClaimDictionaries.Add(claimEntity);
        await _context.SaveChangesAsync();

        claimModel.ClaimId = claimEntity.ClaimId;
        return claimModel;
    }

    public async Task<ClaimDictionaryModel> GetClaimByIdAsync(int claimId)
    {
        var claimEntity = await _context.ClaimDictionaries.FindAsync(claimId);
        if (claimEntity == null) return null;

        return new ClaimDictionaryModel
        {
            ClaimId = claimEntity.ClaimId,
            ClaimType = claimEntity.ClaimType,
            ClaimValue = claimEntity.ClaimValue
        };
    }

    public async Task<IEnumerable<ClaimDictionaryModel>> GetAllClaimsAsync()
    {
        return await _context.ClaimDictionaries
            .Select(c => new ClaimDictionaryModel
            {
                ClaimId = c.ClaimId,
                ClaimType = c.ClaimType,
                ClaimValue = c.ClaimValue
            })
            .ToListAsync();
    }

    public async Task<ClaimDictionaryModel> UpdateClaimAsync(ClaimDictionaryModel claimModel)
    {
        var claimEntity = await _context.ClaimDictionaries.FindAsync(claimModel.ClaimId);
        if (claimEntity == null) return null;

        claimEntity.ClaimType = claimModel.ClaimType;
        claimEntity.ClaimValue = claimModel.ClaimValue;

        await _context.SaveChangesAsync();
        return claimModel;
    }

    public async Task<bool> DeleteClaimAsync(int claimId)
    {
        var claimEntity = await _context.ClaimDictionaries.FindAsync(claimId);
        if (claimEntity == null) return false;

        // Remove the claim from AspNetUserClaims and AspNetRoleClaims
        var userClaims = _context.UserClaims.Where(uc => uc.ClaimType == claimEntity.ClaimType && uc.ClaimValue == claimEntity.ClaimValue);
        var roleClaims = _context.RoleClaims.Where(rc => rc.ClaimType == claimEntity.ClaimType && rc.ClaimValue == claimEntity.ClaimValue);

        _context.UserClaims.RemoveRange(userClaims);
        _context.RoleClaims.RemoveRange(roleClaims);

        _context.ClaimDictionaries.Remove(claimEntity);
        await _context.SaveChangesAsync();
        return true;
    }
}
