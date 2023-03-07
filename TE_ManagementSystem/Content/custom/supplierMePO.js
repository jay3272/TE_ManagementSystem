var supplierColumns = [{ name: 'ID', minWidth: '100px' }, { name: 'Name', minWidth: '320px' }, { name: 'Email', minWidth: '200px' }, { name: 'Phone', minWidth: '100px' }];
var suppliers = new Array();

var kindColumns = [{ name: 'ID', minWidth: '100px' }, { name: 'Name', minWidth: '320px' }];
var kinds = new Array();

var customerColumns = [{ name: 'ID', minWidth: '100px' }, { name: 'Name', minWidth: '320px' }, { name: 'CustCode', minWidth: '200px' }];
var customers = new Array();

var kpnColumns = [{ name: 'ID', minWidth: '100px' }, { name: 'Name', minWidth: '320px' }];
var kpns = new Array();

var meProductColumns = [{ name: 'ID', minWidth: '100px' }, { name: 'ProdName', minWidth: '320px' }];
var meProducts = new Array();

var locationColumns = [{ name: 'ID', minWidth: '100px' }, { name: 'Name', minWidth: '320px' }, { name: 'RackPosition', minWidth: '200px' }];
var locations = new Array();

var txtSerialNum = 0;
var clickedTextboxId = "name0";
var clickedIdNum = "";
var x, y;
var _total = 0;
var IsReturn = "false";
function OnTypeSupplierName(param) {
    
    $(param).autocomplete({
        showHeader: true,
        columns: supplierColumns,
        source: suppliers,
        select: function (event, ui) {
            this.value = (ui.item ? ui.item[1] : '');
            $('#idnSupplier').val(ui.item ? ui.item[0] : '');
            $('#supplierMail').val(ui.item ? ui.item[2] : '');
            $('#supplierPhone').val(ui.item ? ui.item[3] : '');
            update_itemTotal();
            return false;
        }

    });
    
}

function OnTypeKindName(param) {

    $(param).mcautocomplete({
        showHeader: true,
        columns: kindColumns,
        source: kinds,
        select: function (event, ui) {
            this.value = (ui.item ? ui.item[1] : '');
            $('#idnKind').val(ui.item ? ui.item[0] : '');
            update_itemTotal();
            return false;
        }

    });

}

function OnTypeCustomerName(param) {

    $(param).mcautocomplete({
        showHeader: true,
        columns: customerColumns,
        source: customers,
        select: function (event, ui) {
            this.value = (ui.item ? ui.item[1] : '');
            $('#idnCustomer').val(ui.item ? ui.item[0] : '');
            $('#custCode').val(ui.item ? ui.item[2] : '');
            update_itemTotal();
            return false;
        }

    });

}

function OnTypeKpnName(param) {

    $(param).mcautocomplete({
        showHeader: true,
        columns: kpnColumns,
        source: kpns,
        select: function (event, ui) {
            this.value = (ui.item ? ui.item[1] : '');
            $('#idnKpn').val(ui.item ? ui.item[0] : '');
            update_itemTotal();
            return false;
        }

    });

}

function OnTypeMeProductName(param) {

    $(param).mcautocomplete({
        showHeader: true,
        columns: meProductColumns,
        source: meProducts,
        select: function (event, ui) {
            this.value = (ui.item ? ui.item[1] : '');
            $('#idnMeProdName').val(ui.item ? ui.item[0] : '');
            update_itemTotal();
            return false;
        }

    });

}

function OnTypeLocationName(param) {

    $(param).mcautocomplete({
        showHeader: true,
        columns: locationColumns,
        source: locations,
        select: function (event, ui) {
            this.value = (ui.item ? ui.item[1] : '');
            $('#idnLocation').val(ui.item ? ui.item[0] : '');
            $('#locationRack').val(ui.item ? ui.item[2] : '');
            update_itemTotal();
            return false;
        }

    });

}

$(function () {
    //OnTypeName('#name0');
    //alert('#' + clickedTextboxId);


    ConfigDialogueCreateSupplier();

});

$(document).ready(function () {

    //$(this).closest('form').find("input[type=text], textarea").val("");

    //alert('iam ready');
    document.getElementById('supplier').focus();

    //$('#name').tooltip('show')

    //$('[data-toggle="tooltip"]').tooltip();
    var actions = $("table td:last-child").html();
    // Append table with add row form on add new button click

    $('#addNewRow').keydown(function (event) {
        //alert('keydown');
        if (event.keyCode == 13) {
            $('#addNewRow').trigger('click');
        }
    });

    $("#addNewRow").click(function (e) {
        //alert('click');
        //var key = e.which;
        //if (key !== 13)  // the enter key code
        //{

        //    return false;
        //}

        //$(this).attr("disabled", "disabled");
        txtSerialNum += 1;
        //alert(txtSerialNum)
        var index = $("table tbody tr:last-child").index();
        //var rowCount = 
        var row = '<tr>' +
            '<td id="SNo' + txtSerialNum + '">' + $('#selectedProducts tr').length + '</td>' +
            '<td style="display:none;"><input type="hidden" name="PurchaseOrderDetail.Index" value="' + txtSerialNum + '" /></td>' +
            '<td style="display:none;"><input type="text" readonly class="form-control classBGcolor" name="PurchaseOrderDetail[' + txtSerialNum + '].ProductId" id="idn' + txtSerialNum + '"></td>' +
            '<td><input type="text" class="form-control" autocomplete="off" name="name' + txtSerialNum + '" id="name' + txtSerialNum + '"></td>' +
            '<td><input type="text"  class="form-control autocomplete="off" classBGcolor" name="PurchaseOrderDetail[' + txtSerialNum + '].PurchasePrice" id="purchasePrice' + txtSerialNum + '"></td>' +
            '<td><input type="text" class="form-control" autocomplete="off" name="PurchaseOrderDetail[' + txtSerialNum + '].Quantity" id="quantity' + txtSerialNum + '"></td>' +
            '<td><select class="form-control" name="PurchaseOrderDetail[' + txtSerialNum + '].IsPack" id="isPack' + txtSerialNum + '"><option value="false">Piece</option><option value="true" selected>Pack</option></select></td>' +
            '<td><input type="text" class="form-control" readonly autocomplete="off" name="PurchaseOrderDetail[' + txtSerialNum + '].PerPack" id="perPack' + txtSerialNum + '"></td>' +

            '<td><input type="text" readonly class="form-control classBGcolor" name="itemTotal' + txtSerialNum + '" id="itemTotal' + txtSerialNum + '"tabindex="-1"></td>' +
            '<td style="display:none;"><select class="form-control" name="PurchaseOrderDetail[' + txtSerialNum + '].SaleType" id="saleType' + txtSerialNum + '"><option value="false" selected>Order</option><option value="true">Return</option></select></td>' +
            '<td><button type="button" id="delete' + txtSerialNum + '" class="delete btn btn-default add-new"> <a class="delete" title="Delete" data-toggle="tooltip"><i class="material-icons">&#xE872;</i></a></button></td>' +
            '</tr>';
        
        //alert(row);
        $("#selectedProducts").append(row);
        //alert(txtSerialNum)

        $("table tbody tr").eq(index + 1).find(".add, .edit").toggle();
        //$('[data-toggle="tooltip"]').tooltip();
        
        document.getElementById('name' + txtSerialNum).focus();
        TriggerBodyEvents();
        
    });


    // Edit row on edit button click
    $(document).on("click", ".edit", function () {
        $(this).parents("tr").find("td:not(:last-child)").each(function () {
            $(this).html('<input type="text" class="form-control" value="' + $(this).text() + '">');
        });
        $(this).parents("tr").find(".add, .edit").toggle();
        $("#addNewRow").attr("disabled", "disabled");
    });



    //$(document).on("keyup", "#delete1", function (event) {
    //    if (event.keyCode == 13) {
    //        $("#delete1").trigger('click');
    //    }
    //});



    // Delete row on delete button click
    //$(document).on("click", "#delete1", function () {
    //    $(this).parents("tr").remove();
    //    $("#addNewRow").removeAttr("disabled");
    //    update_itemTotal();
    //});
    $(document).on("keypress", "form", function (event) {
        return event.keyCode != 13;
    });
    //$('td[id^="' + value +'"]')         "[id^='quantity']"


    TriggerFooterEvents();

    //$("[id^='quantity']").keydown(function (e) {
    //    // Allow: backspace, delete, tab, escape, enter and .(45) and -(46)
    //    if ($.inArray(e.keyCode, [8, 9, 27, 13, 110]) !== -1 ||

    //        // Allow: Ctrl+A, Command+A
    //        (e.keyCode === 65 && (e.ctrlKey === true || e.metaKey === true)) ||
    //        // Allow: home, end, left, right, down, up
    //        (e.keyCode >= 35 && e.keyCode <= 40)) {
    //        // let it happen, don't do anything

    //        return;
    //    }
    //    // Ensure that it is a number and stop the keypress
    //    if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
    //        e.preventDefault();
    //    }
    //});

    $('#CreatePO').keydown(function (event) {
        if (event.keyCode == 13) {
            $('#CreatePO').trigger('click');
        }
    });
    $('#CreatePO').click(function () {

        if ($('#idnSupplier').val() == "") {
            alert('Supplier not found. Please select supplier from list or add new');
            return false;

        }

         //-----------------------------------------------------------------v-product stock check-v--------------------------
        //var EnteredQty1 = 0;
        //var EnteredProductId1 = 0;
        //var tblProductStock1 = 0;
        //var tblProductId = 0;
        //var saleType1 = false;
        //var idx1 = 0;
        //var isFalse = true;
        //$('#selectedProducts tbody tr').each(function () {
        //    idx1 += 1;
        //    EnteredQty1 = $(this).closest("tr").find("[id^='quantity']").val();
        //    EnteredProductId1 = $(this).closest("tr").find("[id^='idn']").val().trim();
        //    saleType1 = $(this).closest("tr").find("[id^='saleType']").val().trim();
        //    $('#productsTable tr').each(function () {
        //        tblProductId1 = $(this).find(".ProductId").html();
        //        if (typeof tblProductId1 != 'undefined') {
        //            if (EnteredProductId1 == tblProductId1.trim()) {
        //                //alert('abc');
        //                tblProductStock1 = $(this).find(".stock").html().trim();
        //            }
        //        }
        //    });
        //    if (Number(EnteredQty1) > Number(tblProductStock1) && saleType1 == "false") {
        //        alert("Item # " + idx1 + " available stock is " + tblProductStock1);
        //        //$(this).closest("tr").find("[id^='quantity']").val(tblProductStock1);
        //        isFalse = false;
        //        return false;
        //    }
        //});
        //if (isFalse == false) {
        //    return false;
        //}
        //-----------------------------------------------------------------^-product stock check-^--------------------------

        //alert($('#ItemsTotal').val());
        var wentRight = 1;
        var InvalidproductName;
        var idx = 0;



        if (isNaN($('#total').val())) {
            alert('Total is not valid');
            return false;
        }
        //
        if (isNaN($('#balance').val())) {
            alert('Balance is not valid');
            return false;
        }

        $('#selectedProducts > tbody  > tr').each(function () {
            idx += 1;
            var price = $(this).find("[id^='purchasePrice']").val();
            InvalidproductName = $(this).find("[id^='name']").val();
            //alert(price);
            if (!price) {
                //alert(price + " returning");
                wentRight = 0;
                return false;

            }
        });

        if (wentRight == 0) {
            //alert("item # " + idx + " " + InvalidproductName + " is not a valid product name. Please select valid product from product list");
            alert("(Item # " + idx + ") " + InvalidproductName + " Please select appropriate product name from list");
            return false;
        }

        //if ($('#ItemsTotal').val() == 0) {
        //    alert('Please add at least one product to proceed');
        //    return;
        //}

        if ($('#discount').val().trim() == "") {
            $('#discount').val(0);
        }
        if ($('#paid').val().trim() == "") {
            $('#paid').val(0);
        }
        //if ($('#ItemsTotal').val().trim() == "") {
        //    $('#ItemsTotal').val(0);
        //}
        //if ($('#ReturnItemsTotal').val().trim() == "") {
        //    $('#ReturnItemsTotal').val(0);
        //}

        //$("#CreatePO").attr("disabled", true);
        $('form').preventDoubleSubmission();

    });

    jQuery.fn.preventDoubleSubmission = function () {
        $(this).on('submit', function (e) {
            var $form = $(this);
            //alert('abc');
            if ($form.data('submitted') === true) {
                // Previously submitted - don't submit again
                e.preventDefault();
            } else {
                // Mark it so that the next submit can be ignored
                $form.data('submitted', true);
            }
        });

        // Keep chainability
        return this;
    };

    //$(document).on("click", "OpenNewSuppForm", function () {
    //    $("#dialog-CreateSupplier").dialog("open");
    //});

    $('#OpenNewSuppForm').click(function () {
        
        $("#dialog-CreateSupplier").dialog("open");
    });

    $('#btnCreateNewSupp').click(function () {

        $("#dialog-CreateSupplier").dialog("close");
        //$('#idnSupplier').val(SupplierId);
        var contents = $("#NewSupplierId").val();
        $("#idnSupplier").val(contents);

        contents = $("#NewSupplierName").val();

        $("#supplier").val(contents);

        contents = $("#NewSupplierAddress").val();
        $("#supplierAddress").val(contents);

        $("#PreviousBalance").val(0.00);
        update_itemTotal();
        //alert(contents);
    });

    //$('[id^="saleType"]').change(function () {
    //    update_itemTotal();
    //});

    HideOnSaleType();   
    
});

function HideOnSaleType() {

    if (IsReturn == 'true') {//if (IsReturn == 'True') { c# model uses 'T'rue and java script user 't'rue
        //$('#name').prop('selectedIndex', 0);

        $('#rOrderTotal').hide();
        $('#rDiscount').hide();
    }
    else {

        $('#rReturnTotal').hide();
    }

}
function TriggerBodyEvents() {
    //alert('#name' + txtSerialNum);
    OnTypeName('#name' + txtSerialNum);

    $('#quantity' + txtSerialNum).keyup(function () {
        update_itemTotal();
    });
    $('#purchasePrice' + txtSerialNum).keyup(function () {
        update_itemTotal();
    });
    //$('#delete' + txtSerialNum).keyup(function () {
    //    update_itemTotal();
    //});
    $('#delete' + txtSerialNum).keyup(function (event) {
        //alert(txtSerialNum);
        if (event.keyCode == 13) {
            //var focused = document.activeElement;
            //alert('#' + document.activeElement.id);
            //$('#delete' + txtSerialNum).trigger('click');
            //$('#'+focused.id).trigger('click');
            $('#' + document.activeElement.id).trigger('click');
        }
    });
    $('#delete' + txtSerialNum).click(function () {
        //alert(txtSerialNum);
        $(this).parents("tr").remove();
        $("#addNewRow").removeAttr("disabled");
        update_itemTotal();
    });
    //
    //$('#PreviousBalance').keyup(function () {
    //    alert("fff");
    //    update_itemTotal();
    //});
    $('#quantity' + txtSerialNum).keydown(function (e) {
        // Allow: backspace, delete, tab, escape, enter and .
        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110]) !== -1 ||
            // Allow: Ctrl+A, Command+A
            (e.keyCode === 65 && (e.ctrlKey === true || e.metaKey === true)) ||
            // Allow: home, end, left, right, down, up
            (e.keyCode >= 35 && e.keyCode <= 40)) {
            // let it happen, don't do anything
            return;
        }
        // Ensure that it is a number and stop the keypress
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
        }
    });
    $('#saleType' + txtSerialNum).change(function () {
        //var end = this.value;
        //var firstDropVal = $('#saleType').val();
        update_itemTotal();
    });
    $('#isPack' + txtSerialNum).change(function () {
        //if ($('#isPack' + txtSerialNum).val() == "false") {//false=piece true=PerPack
        //    $('#perPack' + txtSerialNum).val(0);
        //}
        update_itemTotal();
    });
    $('#perPack' + txtSerialNum).keyup(function () {
        //var end = this.value;
        //var firstDropVal = $('#saleType').val();
        update_itemTotal();
    });
    //packing0
}
function TriggerFooterEvents() {
    $("#discount,#PreviousBalance").keyup(function () {
        //alert("afasf");
        update_itemTotal();
    });

    $("#paid").keyup(function () {
        //alert(_total);
        var paid = $('#paid').val();
        var balance = _total - paid;
        $('#balance').val(balance.toFixed(2));

    });

}
function ConfigDialogueCreateSupplier() {
    //alert("create Supplier configured");
    $("#dialog-CreateSupplier").dialog({
        title: '',
        modal: true,
        autoOpen: false,
        resizable: true,
        draggable: true,
        height: '480',
        width: '600',
        closeOnEscape: true,
        //position: {
        //    my: 'left top',
        //    at: 'left bottom',
        //    of: $(param)
        //},
        open: function (event, ui) {
            $(".ui-dialog-titlebar-close", ui.dialog | ui).show();
            $('#dialog-CreateSupplier').css('overflow', 'hidden'); //this line does the actual hiding
        }


    });

}
$('td:first-child').each(function () {
    console.log($(this).text());
});

function update_itemTotal() {
    var ItemsTotal = 0;
    var ReturnsTotal = 0;
    var orderQty = 0;
    var orderQtyPiece = 0;
    var returnQty = 0;
    var returnQtyPiece = 0;
    var SN = 0;
    $("#OrderTotal").val('Order Total(' + parseInt(orderQty) + ')');
    $("#ReturnTotal").val('Return Total(' + parseInt(returnQty) + ')');
    $('#selectedProducts > tbody  > tr').each(function () {
        if (IsReturn == 'true') {//if (IsReturn == 'True') { c# model uses 'T'rue and java script user 't'rue
            //$('#name').prop('selectedIndex', 0);

            $(this).find("[id^='saleType']").prop('selectedIndex', 1);
            //alert('aaa');
        }
        else {

            $(this).find("[id^='saleType']").prop('selectedIndex', 0);
            //alert(IsOrder);
        }

        SN += 1;
        $(this).find("[id^='SNo']").text(SN);

        var qty = 0;
        var price = 0;

        //alert($(this).find("[id^='saleType']").val());
        if ($(this).find("[id^='saleType']").val() == "false") {
            qty = $(this).find("[id^='quantity']").val();
            if (!(qty)) { qty = 0; }
            price = $(this).find("[id^='purchasePrice']").val();
            
            var itemAmount = (qty * price);
            if ($(this).find("[id^='isPack']").val() == "true") {//false=item true=PerPack
                itemAmount = itemAmount * $(this).find("[id^='perPack']").val();
                orderQty += parseInt(qty);
            }
            else {
                orderQtyPiece += parseInt(qty);
            }
            ItemsTotal += itemAmount;
            $(this).find("[id^='itemTotal']").val(itemAmount.toFixed(2));
        } else {
            //alert($('#saleType').val());
            qty = $(this).find("[id^='quantity']").val();
            if (!(qty)) { qty = 0; }
            price = $(this).find("[id^='purchasePrice']").val();
            
            var ItemAmount = (qty * price);
            if ($(this).find("[id^='isPack']").val() == "true") {//false=item true=PerPack
                ItemAmount = ItemAmount * $(this).find("[id^='perPack']").val();
                returnQty += parseInt(qty);
                //alert(ItemAmount);
            } else {
                returnQtyPiece += parseInt(qty);
            }
            ReturnsTotal += ItemAmount;
            $(this).find("[id^='itemTotal']").val(ItemAmount.toFixed(2));


        }

        $("#OrderTotal").val('Order Total(' + parseInt(orderQty) + ' pack, ' + parseInt(orderQtyPiece) + ' piece)');
        $("#ReturnTotal").val('Return Total(' + parseInt(returnQty) + ' pack, ' + parseInt(returnQtyPiece) + ' piece)');
    });

    //$('#OpenNewSuppForm').click(function () {
    //    $("#dialog-CreateSupplier").dialog("open");
    //});

    //$('[id^="quantity"]').change(function () {
    //    alert("");
    //});

    //jQuery('[id^="quantity"]').on('change input propertychange paste', function () {
    //    alert("c");
    //});

    /////////////////////////////////
    //$('[id^="quantity"]').blur(function () {
    //    //var inputObj = this;
    //    var EnteredQty = 0;
    //    var EnteredProductId = 0;
    //    var tblProductStock = 0;
    //    var tblProductId = 0;
    //    var saleType = false;
    //    EnteredQty = this.value.trim();
    //    //alert(this.value.trim());
    //    EnteredProductId = $(this).closest("tr").find("[id^='idn']").val().trim();
    //    saleType = $(this).closest("tr").find("[id^='saleType']").val().trim();


    //    $('#productsTable tr').each(function () {
    //        tblProductId = $(this).find(".ProductId").html();

    //        if (typeof tblProductId != 'undefined') {
    //            if (EnteredProductId == tblProductId.trim()) {
    //                //alert('abc');
    //                tblProductStock = $(this).find(".stock").html().trim();
    //                //alert(tblProductStock.trim());
    //                //if (EnteredQty.trim() > tblProductStock.trim()) {
    //                //    alert("available stock is " + tblProductStock);
    //                //    return false; 
    //                //}
    //                //return false;
    //            }
    //        }
    //    });
    //    //alert(EnteredQty.trim());
    //    //alert(tblProductStock.trim());
    //    if (Number(EnteredQty) > Number(tblProductStock) && saleType == "false") {

    //        alert("available stock is " + tblProductStock);
    //        $(this).closest("tr").find("[id^='quantity']").val(EnteredQty.trim());
    //        //return false;

    //    }

    //    //$('#productsTable .ProductIdd').each(function () {
    //    //    alert($(this).html());
    //    //});

    //});
    /////////////////////////////////////




    //$('[id^="saleType"]').change(function () {
    //    //var end = this.value;
    //    //var firstDropVal = $('#saleType').val();
    //    update_itemTotal();
    //});

    //$('[id^="quantity"]').change(function () {
    //    //var end = this.value;
    //    //var firstDropVal = $('#saleType').val();

    //    update_itemTotal();
    //});


    //jQuery('[id^="quantity"]').on('input propertychange paste', function () {
    //    alert("qunatity was entered");
    //});

    //if (ItemsTotal > 0 || ReturnsTotal > 0) {
    //    $('#dvCalculations').show();
    //} else {
    //    $('#dvCalculations').hide();
    //}

    $('#ItemsTotal').val(ItemsTotal.toFixed(2));//for input element
    $('#ReturnItemsTotal').val(ReturnsTotal.toFixed(2));//for input element

    var discount = $('#discount').val();
    var prevBal = parseFloat($("#PreviousBalance").val());
    var subtotal = ItemsTotal - discount - ReturnsTotal;
    var total = ItemsTotal - discount - ReturnsTotal + prevBal;
    //total += $("#PreviousBalance").val();
    //alert(subtotal);
    $('#subtotal').val(subtotal.toFixed(2));
    $('#total').val(total.toFixed(2));
    $('#paid').val(total.toFixed(2));
    _total = total;
    //alert(ItemsTotal + ", " + discount + ", " + ReturnsTotal + ", " + prevBal);
    var paid = $('#paid').val();
    var balance = _total - paid;
    $('#balance').val(balance.toFixed(2));

    //$('#ItemsTotal > tbody > tr > td').val(ItemsTotal);
    //just update the total to sum
    //$('.total').text(ItemsTotal);
}

$(function () {
    $.widget("custom.combobox", {
        _create: function () {
            this.wrapper = $("<span>")
                .addClass("custom-combobox")
                .insertAfter(this.element);

            this.element.hide();
            this._createAutocomplete();
            this._createShowAllButton();
        },

        _createAutocomplete: function () {
            var selected = this.element.children(":selected"),
                value = selected.val() ? selected.text() : "";

            this.input = $("<input>")
                .appendTo(this.wrapper)
                .val(value)
                .attr("title", "")
                .addClass("custom-combobox-input ui-widget ui-widget-content ui-state-default ui-corner-left")
                .autocomplete({
                    delay: 0,
                    minLength: 0,
                    source: this._source.bind(this)
                })
                .tooltip({
                    classes: {
                        "ui-tooltip": "ui-state-highlight"
                    }
                });

            this._on(this.input, {
                autocompleteselect: function (event, ui) {
                    ui.item.option.selected = true;
                    this._trigger("select", event, {
                        item: ui.item.option
                    });
                },

                autocompletechange: "_removeIfInvalid"
            });
        },

        _createShowAllButton: function () {
            var input = this.input,
                wasOpen = false;

            $("<a>")
                .attr("tabIndex", -1)
                .attr("title", "Show All Items")
                .tooltip()
                .appendTo(this.wrapper)
                .button({
                    icons: {
                        primary: "ui-icon-triangle-1-s"
                    },
                    text: false
                })
                .removeClass("ui-corner-all")
                .addClass("custom-combobox-toggle ui-corner-right")
                .on("mousedown", function () {
                    wasOpen = input.autocomplete("widget").is(":visible");
                })
                .on("click", function () {
                    input.trigger("focus");

                    // Close if already visible
                    if (wasOpen) {
                        return;
                    }

                    // Pass empty string as value to search for, displaying all results
                    input.autocomplete("search", "");
                });
        },

        _source: function (request, response) {
            var matcher = new RegExp($.ui.autocomplete.escapeRegex(request.term), "i");
            response(this.element.children("option").map(function () {
                var text = $(this).text();
                if (this.value && (!request.term || matcher.test(text)))
                    return {
                        label: text,
                        value: text,
                        option: this
                    };
            }));
        },

        _removeIfInvalid: function (event, ui) {

            // Selected an item, nothing to do
            if (ui.item) {
                return;
            }

            // Search for a match (case-insensitive)
            var value = this.input.val(),
                valueLowerCase = value.toLowerCase(),
                valid = false;
            this.element.children("option").each(function () {
                if ($(this).text().toLowerCase() === valueLowerCase) {
                    this.selected = valid = true;
                    return false;
                }
            });

            // Found a match, nothing to do
            if (valid) {
                return;
            }

            // Remove invalid value
            this.input
                .val("")
                .attr("title", value + " didn't match any item")
                .tooltip("open");
            this.element.val("");
            this._delay(function () {
                this.input.tooltip("close").attr("title", "");
            }, 2500);
            this.input.autocomplete("instance").term = "";
        },

        _destroy: function () {
            this.wrapper.remove();
            this.element.show();
        }
    });

    $("#combobox").combobox();
    $("#combobox2").combobox();
    $("#combobox3").combobox();
    $("#combobox4").combobox();
    $("#toggle").on("click", function () {
        $("#combobox").toggle();
    });
});