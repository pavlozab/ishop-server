using System.Linq;
using Entities;

namespace Data.Dto
{
    public class QueryMetaDto
    {
        public string SortBy { get; set; }
        public string SortType { get; set; }
        public int Offset { get; set; }
        public int Limit { get; set; }
        public string Search { get; set; }
        
        public string Brands { get; set; }
        public string Memories { get; set; }
        public string Colors { get; set; }

        public QueryMetaDto()
        {
            Search = "";
            SortBy = "Title";
            SortType = "ASC";
            Offset = 0;
            Limit = 10;
            Brands = "";
            Memories = "";
            Colors = "";
        }

        public QueryMetaDto(QueryMetaDto filter)
        {
            Search = filter.Search;
            Validate();
        }
        
        public void Validate()
        {
            SortBy = typeof(Product).GetProperties()
                .Select(obj => obj.Name)
                .Contains(SortBy) ? SortBy : "Title";
            
            SortType = SortType == "DESC" ? "DESC" : "ASC";
            Offset = Offset < 0 ? 0 : Offset;
            Limit = Limit > 10 ? 10 : Limit;
        }
    }
}