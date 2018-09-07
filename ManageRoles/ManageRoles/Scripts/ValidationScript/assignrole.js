$('#MenuId').change(function ()
{
    if ($("#MenuId").val() !== "")
    {
        var url = "/AssignRolestoMenu/GetSubMenuList";
        $.getJSON(url, { menuid: $("#MenuId").val() }, function (data)
        {
            if (data) {
                $("#SubMenuId").empty();
               
                $.each(data, function (index, optionData) {
                    $("#SubMenuId").append("<option text='" + optionData.SubMenuName + "' value='" + optionData.SubMenuId + "'>" + optionData.SubMenuName + "</option>");
                });
            }
        });
    }
});


function rebindSubmenu(subMenuId) {
    if ($("#MenuId").val() != "-1" && $("#MenuId").val() != "0" && $("#SubMenuId").val() == "-1") {
        var url = "/AssignRolestoMenu/GetSubMenuList";
        $.getJSON(url, { menuid: $("#MenuId").val() }, function (data) {
            if (data) {
                $("#SubMenuId").empty();
               
                $.each(data, function (index, optionData) {
                    $("#SubMenuId").append("<option text='" + optionData.SubMenuName + "' value='" + optionData.SubMenuId + "'>" + optionData.SubMenuName + "</option>");
                });

                $("#SubMenuId").val(subMenuId);
            }
        });
    }
}


