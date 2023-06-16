
    function NavigateToProductsPage(id) {
        window.location.href = "/home/GetProductAndCategoriesByStoreId?storeId=" + id;
       //var url = '@Url.Action("GetProductAndCategoriesByStoreId","Home", new {id = "__id__"})';
       //window.location.href = url.replace('__id__', "storeId=" + id);
    }
