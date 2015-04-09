using System.Collections.Generic;
using System.Linq;
using Chaos.Cockpit.Core.Api.Result;
using Chaos.Portal.Core;
using Chaos.Portal.Core.Data.Model;
using Chaos.Portal.Core.Extension;

namespace Chaos.Cockpit.Core.Api.Endpoints
{
  public class SearchExtension : AExtension
  {
    public SearchExtension(IPortalApplication portalApplication) : base(portalApplication)
    {

    }

    public IPagedResult<SearchResult> Simple(string q, uint pageIndex = 0, uint pageSize = 10)
    {
      var results = new List<SearchResult>
        {
          new SearchResult {Identifier = "00000000-0000-0000-0000-000000000001", Title = "test1"},
          new SearchResult {Identifier = "00000000-0000-0000-0000-000000000002", Title = "test2"},
          new SearchResult {Identifier = "00000000-0000-0000-0000-000000000003", Title = "test3"},
          new SearchResult {Identifier = "00000000-0000-0000-0000-000000000004", Title = "test4"},
          new SearchResult {Identifier = "00000000-0000-0000-0000-000000000005", Title = "test5"},
          new SearchResult {Identifier = "00000000-0000-0000-0000-000000000006", Title = "test6"},
          new SearchResult {Identifier = "00000000-0000-0000-0000-000000000007", Title = "test7"},
          new SearchResult {Identifier = "00000000-0000-0000-0000-000000000008", Title = "test8"},
          new SearchResult {Identifier = "00000000-0000-0000-0000-000000000009", Title = "test9"},
          new SearchResult {Identifier = "00000000-0000-0000-0000-000000000010", Title = "test10"},
          new SearchResult {Identifier = "00000000-0000-0000-0000-000000000011", Title = "test11"},
          new SearchResult {Identifier = "00000000-0000-0000-0000-000000000012", Title = "test12"},
          new SearchResult {Identifier = "00000000-0000-0000-0000-000000000013", Title = "test13"},
          new SearchResult {Identifier = "00000000-0000-0000-0000-000000000014", Title = "test14"},
          new SearchResult {Identifier = "00000000-0000-0000-0000-000000000015", Title = "test15"},
          new SearchResult {Identifier = "00000000-0000-0000-0000-000000000016", Title = "test16"},
          new SearchResult {Identifier = "00000000-0000-0000-0000-000000000017", Title = "test17"},
          new SearchResult {Identifier = "00000000-0000-0000-0000-000000000018", Title = "test18"},
          new SearchResult {Identifier = "00000000-0000-0000-0000-000000000019", Title = "test19"},
          new SearchResult {Identifier = "00000000-0000-0000-0000-000000000020", Title = "test20"}
        };

      return new PagedResult<SearchResult>((uint) results.Count, pageIndex, results.Skip((int) (pageIndex * pageSize)).Take((int) pageSize));
    }
  }
}