using System.Threading.Tasks;
using CG.Enums;

namespace CG.Interfaces;

public interface IFilter
{
    string Name { get; set; }
    FilterState State { get; set; }
    Task Apply(WriteableBitmap source);
}