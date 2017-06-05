using NaSpacerDo.Domain.Models;
using PagedList;
using NaSpacerDo.Logic;

namespace NaSpacerDo.ViewModels
{
    public class CompanyIndexViewModel
    {
        private int page;

        public CompanyIndexViewModel()
        {
            SearchCriteria = new CompanySearchCriteria();
        }

        public IPagedList<Company> Companies { get; set; }

        public CompanySearchCriteria SearchCriteria { get; set; }

        public int Page
        {
            get { return page < 1 ? 1 : page; }
            set { page = value; }
        }
    }
}