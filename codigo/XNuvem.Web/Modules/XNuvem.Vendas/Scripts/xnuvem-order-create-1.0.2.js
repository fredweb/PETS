/// <reference path="jquery-2.2.3.js" />
/// <reference path="jquery.validate.js" />
/// <reference path="../Content/plugins/select2/select2.js" />
/// <reference path="globalize.js" />
/****************************************************************************************
 *
 * Autor: Marvin Mendes
 * Copyright (c) 2016  
 * 
/****************************************************************************************/

var currentBusinessPartner = null;
var currentCreditBalance = null;

$(function () {
    var showLastOrder = function (o) {
        var box = {
            begin: '<div class="box"><div class="box-body">',
            end: '</div></div>',
            footer: {
                begin: '<div class="box-footer">',
                end: '</div>'
            }
        };
        var container = '';
        container += '<strong>Nº interno: ' + o.iDocEntry + '</strong><br />';
        container += '<strong>Data de lançamento: ' + XNuvem.formatDate(o.DocDate) + '</strong><br />';
        container += '<strong>Cliente: ' + o.CardCode + ' - ' + o.CardName + '</strong><br />';

        container += box.begin;
        container += '<div class="responsible-table xnuvem-message">';
        container += '<table class="table table-bordered table-hover">';
        container += '<thead><tr><td class="col-xs-10"><strong>Item</strong></td><td class="col-xs-2 text-right"><strong>Quantidade</strong></td></thead>';
        container += '<tbody>';
        $.each(o.Lines, function (i, l) {
            container += '<tr>';
            container += '<td>' + l.ItemCode + ' - ' + l.ItemName + '</td>';
            container += '<td class="text-right">' + l.Quantity + '</td>';
            container += '</tr>';
        });
        container += '</tbody>';
        container += '</table>';
        container += '</div>'; // responsible table
        container += box.end;
        container += '<strong>Total: ' + XNuvem.formatCurrency(o.DocTotal) + '</strong><br />'

        XNuvem.messageBox.showInfo("Último pedido", container, function () {
            $.each(o.Lines, function (i, l) {
                var selector = '[data-xn-in_itemcode="' + l.ItemCode + '"]';
                $(selector).val(l.Quantity);
            });
            refreshTotalAll();
        });
    }

    var businessPartnerListFormatter = function (c) {
        if (c.loading) return c.text;
        var htm =
            '<div class="clearfix">' +
                '<div><strong>' + c.CardCode + ' - ' + c.CardName + '</strong></div>' +
                '<div class="small"><span>' + c.CardFName + '</span></div>' +
                '<div class="small"><p>' +
                    c.Address + ', ' + c.StreetNo + ' - ' + c.Building + ', ' + c.Block + '<br />' +
                    'Cidade: ' + c.City + ' - ' + c.State + ' CEP: ' + c.ZipCode +
                '</p></div>' +
            '</div>';
        return htm;
    };

    // loadListItems(ln: ListNum)
    var loadListItems = function (ln) {
        var $container = $("#ItemList");
        $.ajax({
            url: urlItems,
            method: 'POST',
            cache: false,
            data: { listNum: ln }
        }).done(function (xhr) {
            $("#waitCustomer").css("display", "none");
            $container.html(xhr);
            initItemsSection();
        }).fail(function (xhr, text) {
            $("#waitCustomer").css("display", "none");
            if (xhr.responseText) {
                XNuvem.statusMessage.showError("Erro!", xhr.responseText);
            } else {
                XNuvem.statusMessage.showError("Erro!", "Não foi possível conectar com o servidor.");
            }
        });
    }

    var customerUrl = $("#CardCode").attr("data-xn-url");
    $("#CardCode").select2({
        language: 'pt-BR',
        placeholder: 'Selecione...',
        ajax: {
            url: customerUrl,
            dataType: 'json',
            delay: 250,
            data: function (params) {
                return {
                    q: params.term, // search term
                    start: params.start || 0
                };
            },
            processResults: function (data, params) {
                // parse the results into the format expected by Select2
                // since we are using custom formatting functions we do not need to
                // alter the remote JSON data, except to indicate that infinite
                // scrolling can be used
                params.start = (params.start || 0) + 30;

                return {
                    results: data.results,
                    pagination: {
                        more: (params.start + 30) < data.total
                    }
                };
            },
            cache: true
        },
        escapeMarkup: function (markup) { return markup; },
        minimumInputLength: 3,
        templateResult: businessPartnerListFormatter
    }).on("select2:select", function (e) {
        currentBusinessPartner = null;
        currentCreditBalance = null;

        $("#CustomerCard").css('visibility', 'hidden');
        $("#waitCustomer").css("display", "block");
        // Reset select2 pay
        $("#PeyMethod").empty().trigger('change');
        $("#GroupNum").empty().trigger('change');

        if (!e.params || !e.params.data) return;

        //Set new data
        currentBusinessPartner = e.params.data;
        var c = currentBusinessPartner;
        checkBalanceLimit(c.CardCode);
        //Visual data
        $("#LabelCardCode").html(c.CardCode);
        $("#LabelCardFName").html(c.CardFName);
        $("#LabelAddress").html(c.Address);
        $("#LabelStreetNo").html(c.StreetNo);
        $("#LabelBuilding").html(c.Building);
        $("#LabelBlock").html(c.Block);
        $("#LabelCity").html(c.City);
        $("#LabelState").html(c.State);
        $("#LabelZipCode").html(c.ZipCode);
        $("#LabelCpfCnpj").html(c.CpfCnpj);
        $("#LabelInscricao").html(c.InscricaoEstadual);
        $("#LabelCreditLine").html(XNuvem.formatCurrency(c.CreditLine));
        $("#LabelDebtLine").html(XNuvem.formatCurrency(c.DebtLine));
        $("#LabelLimitBalance").removeClass('text-red').removeClass('text-blue').addClass(c.LimitBalance < 0 ? 'text-red' : 'text-blue');
        $("#LabelLimitBalance").html(XNuvem.formatCurrency(c.LimitBalance));
        var detailUrl = $("#ButtonDetail").attr('data-xn-url');
        $("#ButtonDetail").attr('href', XNuvem.pathCombine(detailUrl, c.CardCode));

        $("#ListName").val(c.ListName);
        if ((c.PymCode || "-1") !== "-1") {
            $("#PeyMethod").append('<option value="' + c.PymCode + '" selected>' + c.PymCode + '</option>').trigger('change');
        }
        $("#GroupNum").append('<option value="' + c.GroupNum + '" selected>' + c.PymntGroup + '</option>').trigger('change');

        //Initialize list items
        loadListItems(c.ListNum);

        $("#CustomerCard").css('visibility', 'visible');
    });

    var groupNumUrl = $("#GroupNum").attr("data-xn-url");
    $("#GroupNum").select2({
        language: 'pt-BR',
        placeholder: 'Selecione...',
        minimumResultsForSearch: Infinity,
        ajax: {
            url: groupNumUrl,
            dataType: 'json',
            delay: 250,
            data: function (params) {
                return {
                    cardCode: $("#CardCode").val(),
                    q: params.term
                };
            },
            cache: true
        }
    });

    var payMethodsUrl = $("#PeyMethod").attr("data-xn-url");
    $("#PeyMethod").select2({
        language: 'pt-BR',
        placeholder: 'Selecione...',
        minimumResultsForSearch: Infinity,
        ajax: {
            url: payMethodsUrl,
            dataType: 'json',
            delay: 250,
            data: function () {
                return { cardCode: $("#CardCode").val() };
            },
            cache: true
        }
    });

    $("#ButtonFinantial").click(function (e) {
        e.preventDefault();
        var that = $(this);
        var urlAjax = that.attr("href");
        var overlay = that.parents(".overlay-wrapper").find(".overlay");
        var cardCode = $("#CardCode").val() || "";
        if (cardCode == "") {
            XNuvem.statusMessage.showError("Erro!", "Não foi selecionado um cliente para mostrar detalhes financeiros.");
            return;
        }

        overlay.fadeIn('fast');
        $.ajax({
            method: 'GET',
            url: urlAjax,
            data: {
                id: cardCode
            }
        }).done(function (data) {
                overlay.hide();
                if (data.IsError) {
                    XNuvem.statusMessage.showError("Error", data.Messages.join(". "));
                } else {

                    var b = $('<tbody>');
                    $(data).each(function (i, o) {
                        var r = $('<tr>');
                        r.append('<td>' + o.DocType + '</td>');
                        r.append('<td>' + o.DocNum + '</td>');
                        r.append('<td class="' + (o.DaysFromNow <= 0 ? 'text-blue' : 'text-red') +'">' + o.DueDateString + ' (' + o.DaysFromNow + ')</td>');
                        r.append('<td>' + XNuvem.formatCurrency(o.DocTotal) + '</td>');
                        b.append(r);
                    });

                    var m = $('<table class="table" />');
                    var ms = $('<div class="responsible-table xnuvem-message" />');
                    m.append('<thead><th>Tipo</th><th>Documento</th><th>Vencimento</th><th>Valor</th></thead>');
                    m.append(b);
                    ms.append(m);
                    XNuvem.messageBox.showInfo("Pendências", ms.wrapAll('<div>').parent().html());
                }
            }).fail(function (xhr) {
                overlay.hide();
                if (xhr.responseText) {
                    XNuvem.statusMessage.showError("Erro!", xhr.responseText);
                } else {
                    XNuvem.statusMessage.showError("Erro!", "Não foi possível conectar com o servidor.");
                }
            });
    });

    var refreshTotal = function () {
        var $lineTotal = $("#ItemList table > tbody > tr > td > span[data-xn-linetotal]");
        var docTotal = 0.0;
        $lineTotal.each(function (i, o) {
            var lineTotal = parseFloat($(this).attr('data-xn-value')) || 0;
            docTotal += lineTotal;
        });

        $("#SDocTotal").attr("data-xn-value", docTotal.toFixed(2)).html(XNuvem.formatCurrency(docTotal.toFixed(2)));
    };

    var refreshGroupTotal = function (box) {
        var $box = $(box);
        var $tr = $box.find('table > tbody > tr');
        var $lineTotal = $tr.find('td > span[data-xn-linetotal]');
        var groupTotal = 0.0;
        $lineTotal.each(function (c, o) {
            var $this = $(this);
            var lineTotal = parseFloat($this.attr('data-xn-value')) || 0.0;
            groupTotal += lineTotal;
        });
        var $totalGroup = $box.find('[data-xn-totalgroup]');
        $totalGroup.attr('data-xn-value', groupTotal.toFixed(2)).html(XNuvem.formatCurrency(groupTotal.toFixed(2)));
    };

    var refreshTotalLine = function (tr) {
        var $tr = $(tr);
        var $quantity = $tr.find('td > input[data-xn-quantity]');
        var $price = $tr.find('[data-xn-price]');
        var $lineTotal = $tr.find('[data-xn-linetotal]');
        var price = parseFloat($price.attr('data-xn-value')) || 0.00;
        var quantity = parseFloat($quantity.val()) || 0;
        var lineTotal = (price * quantity).toFixed(2);
        $lineTotal.attr('data-xn-value', lineTotal).html(XNuvem.formatCurrency(lineTotal));
    };

    var refreshTotalAll = function () {
        var $box = $("#ItemList .box[data-xn-group]");
        var $tr = $box.find('table > tbody > tr');
        $tr.each(function (i, o) {
            refreshTotalLine($(this));
        });
        $box.each(function (i, o) {
            refreshGroupTotal($(this));
        });
        refreshTotal();
    };

    var initItemsSection = function () {
        $("[data-xn-btn-itemgroup]").click(function (e) {
            e.preventDefault();
            var $this = $(this);
            var $input = $this.parents(".input-group").find('input[type="text"]');
            var $inputGroup = $this.parents(".box-body").find('table').find('tbody > tr > td > input[type="text"]');
            $inputGroup.val($input.val());
            refreshTotalAll();
        });

        $("[data-xn-itemgroup]").on("keydown", function (e) { // Bind our desired behavior
            // Call when the keyCode equals to (enter)
            if (e.keyCode == 13) {
                e.preventDefault();
                var that = $(this);
                that.parent().find("[data-xn-btn-itemgroup]").click();
            }
        });

        $('[data-xn-toggle="collapse"]').click(function (e) {
            e.preventDefault();
            $(this).parents(".box").find('[data-xn-btn-hide]').focus().click();
        });

        $("#ItemList table > tbody > tr > td > input[data-xn-quantity]").on('change', function (e) {
            refreshTotalLine($(this).parents('tr'));
            refreshGroupTotal($(this).parents('.box'));
            refreshTotal();
        }).on('focus', function () {
            var _this = $(this);
            setTimeout(function () {
                $(_this).select();
            }, 100);
        });
    };

    var resetOrderForm = function () {
        currentBusinessPartner = null;
        currentCreditBalance = null;
        //TODO: Limpar os dados do formulário para uma nova edição
        $("[data-xn-htmlclear]").empty();
        $("#CustomerCard").css('visibility', 'hidden');
        $("#CardCode").empty().trigger('change');
        $("#RotaCode").empty().trigger('change');
        $("#PeyMethod").empty().trigger('change');
        $("#GroupNum").empty().trigger('change');
        $("#ItemList").empty();
        $("#ListName").val('');
        $("#Comments").val('');
    };

    $("#OrderForm").submit(function (e) {
        e.preventDefault();
        var $form = $(this);

        // Validate input values
        $form.validate();
        if (!$form.valid()) {
            XNuvem.statusMessage.showError("Erro!", "Verifique se as informações estão corretas.");
            return;
        }
        if (!currentBusinessPartner) {
            XNuvem.statusMessage.showError("Erro!", "Não foi possível verificar o parceiro de negócio.");
            return;
        }
        if (!currentCreditBalance) {
            XNuvem.statusMessage.showError("Erro!", "Não foi possível verificar a situação financeira do cliente.");
            return;
        }       
        var needApproval = !currentCreditBalance.Approved;
        var approved = $("#Approved").prop('checked');
        var comments = $("#Comments").val() || '';
        
        if (needApproval && approved && comments == '') {
            XNuvem.statusMessage.showError("Erro!", "É necessário informar uma observação para este pedido devido ao status de aprovação.");
            return;
        }

        var rotaName = $("#RotaCode").select2("data")[0].text;
        var pymntGroup = $("#GroupNum").select2("data")[0].text;

        var formData = $form.serializeObject();
        formData.RotaName = rotaName;
        formData.PymntGroup = pymntGroup;
        formData.BusinessPartner = currentBusinessPartner;
        formData.FinancialDetails = currentCreditBalance;
        formData.ListName = currentBusinessPartner.ListName;
        formData.Lines = new Array();
        var doc_total = 0;
        $('#ItemList table > tbody > tr > td input[data-xn-quantity]').each(function (i, o) {
            var el = $(this);
            if (parseFloat(el.val())) {
                var line = {
                    ItemCode: el.attr('data-xn-in_itemcode'),
                    ItemName: el.attr('data-xn-in_itemname'),
                    Quantity: parseFloat(el.val()) || 0,
                    Price: parseFloat(el.attr('data-xn-in_price')) || 0
                };
                formData.Lines.push(line);
                doc_total += (line.Quantity * line.Price);
            }
        });
        if (doc_total <= 0) {
            XNuvem.statusMessage.showError("Erro!", "O valor do pedido não pode ser zero.");
            return;
        };

        if ((currentCreditBalance.Balance + doc_total) > currentCreditBalance.CreditLine) {
            if (approved && comments == '') {
                XNuvem.statusMessage.showError("Erro!", "O pedido ultrapassa o limite de crédito.");
                return;
            }
        }

        if ((currentCreditBalance.Balance + (-1 * currentCreditBalance.ChecksBal) + doc_total) > currentCreditBalance.DebtLine) {
            if (approved && comments == '') {
                XNuvem.statusMessage.showError("Erro!", "O pedido ultrapassa o limite de compromisso.");
                return;
            }
        }

        // End validation

        var sb = $form.find("button[data-xn-save]");
        sb.prop("disabled", true);
        var wtn = $('<span><i class="fa fa-refresh fa-spin"></i>&nbsp;&nbsp;</span>');
        sb.prepend(wtn); // Wainting...
        $.ajax({
            type: $form.attr('method'),
            url: $form.attr('action'),
            data: formData
        }).done(function (data) {
            wtn.remove();
            sb.prop("disabled", false);
            if (data.IsError) {
                XNuvem.statusMessage.showError("Erro!", "Erro: " + data.Messages.join(","));
            } else {
                resetOrderForm();
                if (data.Messages) {
                    XNuvem.statusMessage.showSuccess("Sucesso", data.Messages.join(","));
                } else { // Else sucess default message
                    XNuvem.statusMessage.showSuccess("Sucesso", "Operação completada com êxito.")
                }
            }
        }).fail(function (xhr) {
            wtn.remove();
            sb.prop("disabled", false);
            if (xhr.responseText) {
                XNuvem.statusMessage.showError("Erro interno", xhr.responseText);
            } else {
                XNuvem.statusMessage.showError("Erro", "O servidor não retornou nenhum valor ou não está acessível.");
            }
        });
    }); //$("#OrderForm").submit()

    var checkBalanceLimit = function (cardCode) {
        var $cc = $("#LabelCheckCredit");
        $("#Approved").iCheck('uncheck');
        $cc.empty();
        $.ajax({
            url: $cc.attr('data-xn-url'),
            method: 'POST',
            data: { id: cardCode }
        }).done(function (data) {
            currentCreditBalance = data;
            if (!data.Approved) {
                $cc.html('Este cliente ou grupo de clientes possui pendência(s) financeira(s) e será necessário aprovar o pedido.');
            } else {
                $("#Approved").iCheck('check');
            }
        }).fail(function (xhr) {
            XNuvem.statusMessage.showError("Erro!", "Não foi possível carregar o status de pendências deste cliente.");
        });
    };

    $("#ButtonLastOrder").click(function (e) {
        e.preventDefault();
        var _this = $(this);
        var u = _this.attr("href");
        $.ajax({
            url: u,
            method: 'POST',
            data: {
                cardCode: function () {
                    return $("#CardCode").val();
                }
            }
        }).done(function (o) {
            if (!o) {
                XNuvem.statusMessage.showWarning("Aviso!", "O cliente não possui pedidos via WEB.");
                return;
            }
            showLastOrder(o);
        }).fail(function (xhr) {
            XNuvem.statusMessage.showError("Erro!", "Não foi possível carregar o último pedido ou este cliente não possui um pedido web.");
        });
    });

    $("#Usage").append('<option value="-1">Padrão</option>').trigger("change");
});