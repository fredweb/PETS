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

$(function () {

    var searchTime = 0;
    var searchDelay = 800;

    var searchStart = function (d, v) {
        if (searchTime) {
            clearTimeout(searchTime);
            searchTime = 0;
        }
        searchTime = setTimeout(function () {
            d.search(v).draw();
        }, searchDelay);
    };

    var doSearch = function (d) {
        var val = d.parents(".dataTables_wrapper")
            .find(".dataTables_filter input").val();
        var api = this.dt.dataTable().api();
        api.search(val).draw();
    };

    var renderDocEntry = function (row) {
        if (row["iDocEntry"] && row["DocEntry"]) {
            return row["iDocEntry"] + '<br /><span class="text-blue">(' + row["DocEntry"] + ')</span>';
        } else if (row["iDocEntry"]) {
            return row["iDocEntry"];
        } else {
            return '<span class="text-blue">(' + row["DocEntry"] + ')</span>';
        }
    };
    var statusTexts = {
        "N":  '<a href="#" data-xn-status class="text-red" title="Não aprovado"><i class="fa fa-close"></i></a>',
        "Y":  '<a href="#" data-xn-status class="text-red" title="Cancelado"><i class="fa fa-ban"></i></a>',
        "NW": '<a href="#" data-xn-status class="text-yellow" title="Não confirmado"><i class="fa fa-minus-circle"></i></a>',
        "NO": '<a href="#" data-xn-status class="text-success" title="Confirmado"><i class="fa fa-check"></i></a>',
        "NC": '<a href="#" data-xn-status class="text-blue" title="Faturado"><i class="fa fa-truck"></i></a>'
    };
    var renderDocStatus = function (row) {
        if (row["Canceled"] == "Y") {
            return statusTexts["Y"];
        }
        if (!row["Approved"]) {
            return statusTexts["N"];
        }
        return statusTexts[row["Canceled"] + row["DocStatus"]];
    };
    var dt = $("#OrderList");
    var ajaxUrl = dt.attr("data-xn-url");
    var linkFormat = dt.attr("data-xn-link");
    var linkButton = '<a class="btn btn-flat btn-default btn-xs" href="{0}" title="Editar"><i class="fa fa-arrow-right text-orange"></i></a>&nbsp;&nbsp;';

    dt.dataTable({
        ajax: {
            url: ajaxUrl,
            type: "POST",
            error: function (xhr, error, thrown) {
                XNuvem.statusMessage.showError("Tabela de dados", xhr.responseText);
            },
            data: function (d) {
                d.StatusFilter = $("#StatusFilter").val();
                return d;
            }
        },
        columns: [
            { data: null },             //0
            { data: "iDocEntry" },      //1
            { data: "DocEntry" },       //2
            { data: "DocDate" },        //3
            { data: "RotaCode" },       //4
            { data: "CardCode" },       //5
            { data: "CardName" },       //6
            { data: "FullStatus" },     //7
            { data: "DocTotal" },       //8
            { data: "DocDateString" }   //9
        ],
        columnDefs: [
            { // Checkbox column
                "orderable": false,
                "targets": [0],
                "render": function (data, type, row) {
                    return '&nbsp;&nbsp;<input type="checkbox" data-xn-value="{0}" />'.replace("{0}", row["iDocEntry"]);
                }
            },
            { // DocEntry / iDocEntry column
                "orderable": true,
                "targets": [1],
                "render": function (data, type, row, meta) {
                    if (data) {
                        var u = linkFormat.replace("{iDocEntry}", data);
                        return linkButton.replace("{0}", u) + renderDocEntry(row);
                    } else {
                        return renderDocEntry(row);
                    }
                }
            },
            {
                "orderable": false,
                "visible": false,
                "targets": [2]
            },
            {
                "orderable": true,
                "targets": [3],
                "render": function (data, type, row, meta) {
                    return row.DocDateString;
                }
            },
            {
                "orderable": true,
                "targets": [4,5,6]
            },
            {
                "orderable": false,
                "targets": [7],
                "render": function (data, type, row, meta) {
                    return renderDocStatus(row);
                },
                "className": "text-center"
            },
            {
                "orderable": false,
                "targets": [8],
                "render": function (data, type, row, meta) {
                    return '<strong data-xn-total="' + data + '">' + XNuvem.formatCurrency(data) + '</strong>';
                },
                "className": "text-right"
            },
            {
                "orderable": false,
                "visible": false,
                "targets": [9]
            }
        ],
        "order": [[3, "desc"]],
        pageLength: 25,
        processing: true,
        serverSide: true,
        responsive: true,
        language: {
            "sEmptyTable": "Nenhum registro encontrado",
            "sInfo": "Mostrando de _START_ até _END_ de _TOTAL_ registros",
            "sInfoEmpty": "Mostrando 0 até 0 de 0 registros",
            "sInfoFiltered": "(Filtrados de _MAX_ registros)",
            "sInfoPostFix": "",
            "sInfoThousands": ".",
            "sLengthMenu": "_MENU_ resultados por página",
            "sLoadingRecords": '<i class="fa fa-refresh fa-spin"></i> Carregando...',
            "sProcessing": '<i class="fa fa-refresh fa-spin"></i> Processando...',
            "sZeroRecords": "Nenhum registro encontrado",
            "sSearch": "Pesquisar",
            "oPaginate": {
                "sNext": "Próximo",
                "sPrevious": "Anterior",
                "sFirst": "Primeiro",
                "sLast": "Último"
            },
            "oAria": {
                "sSortAscending": ": Ordenar colunas de forma ascendente",
                "sSortDescending": ": Ordenar colunas de forma descendente"
            }
        }
    });

    $("#CheckAll").iCheck({
        checkboxClass: 'icheckbox_square-blue',
        radioClass: 'iradio_square-blue'
    });

    $('#CheckAll').on('ifChecked', function (event) {
        $('.dataTable tbody td input[type="checkbox"]').iCheck('check');
    });

    $('#CheckAll').on('ifUnchecked', function (event) {
        $('.dataTable tbody td input[type="checkbox"]').iCheck('uncheck');
    });

    dt.on("draw.dt", function (xhre) {
        $("#CheckAll").iCheck('uncheck');
        $('.total-wrapper').hide();
        if (typeof $.fn.iCheck !== 'undefined') {
            var allChecks = $('.dataTable td > input[type="checkbox"]');

            $('.dataTable td > input[type="checkbox"]').iCheck({
                checkboxClass: 'icheckbox_square-blue',
                radioClass: 'iradio_square-blue'
            });            

            allChecks.on('ifToggled', function (event) {
                var checkeds = allChecks.filter(':checked');
                if (checkeds.length) {
                    var displayTotal = 0.00;
                    checkeds.each(function (e, o) {
                        var _this = $(this);
                        displayTotal += parseFloat(_this.parents('tr').find('[data-xn-total]').attr('data-xn-total'));
                    });
                    $('#DisplayTotal').html(XNuvem.formatCurrency(displayTotal));
                    $('.total-wrapper').show();
                } else {
                    $('.total-wrapper').hide();
                }
            });

            initStatusColumn();
        }
    });

    dt.parents(".dataTables_wrapper")
                .find(".dataTables_filter input")
                .attr("placeholder", "Pesquisar por...")
		        .unbind() // Unbind previous default bindings
		        .bind("keydown", function (e) { // Bind our desired behavior
		            // Call when the keyCode equals to (enter)
		            if (e.keyCode == 13) {
		                searchStart(dt.api(), $(this).val());
		            }
		            return;
		        })
		        .bind("input", function (e) { // Bind our desired behavior
		            searchStart(dt.api(), $(this).val());
		            return;
		        });

    dt.show();

    $("#StatusFilter").select2().on("change", function (e) {
        dt.api().draw();
    });

    $("#DeleteButton").click(function (e) {
        e.preventDefault();
        var _this = $(this);
        var allCheckeds = $('.dataTable tbody td input[type="checkbox"]').filter(':checked');
        var selectedKeys = [];
        XNuvem.messageBox.showWarning("Aviso",
            "Esta operação não pode ser desfeita. Tem certeza que deseja continuar?",
            function () {
                var allowDelete = true;
                allCheckeds.each(function (i, s) {
                    var value = $(s).attr('data-xn-value');
                    selectedKeys.push(value);
                    if (value == '0') {
                        allowDelete = false;
                    }
                });
                if (!allowDelete) {
                    XNuvem.statusMessage.showError("Erro", "Existem pedidos que não podem ser excluídos.");
                    return;
                }
                $.ajax({
                    method: 'POST',
                    url: _this.attr('href'),
                    data: { keys: selectedKeys.join(';') }
                }).done(function (data) {
                    if (data.IsError) {
                        XNuvem.statusMessage.showError("Erro!", data.Messages.join('. '));
                    } else {
                        XNuvem.statusMessage.showSuccess("Sucesso!", data.Messages.join('. '));
                    }
                    dt.api().draw();
                }).fail(function (xhr) {
                    XNuvem.statusMessage.showError("Erro!", "Não foi possível realizar a exclusão.");
                    dt.api().draw();
                });
            });
    });

    var modalProgressHtml =
        '<div class="modal modal-info" style="display: none;">' +
        '    <div class="modal-dialog">' +
        '    <div class="modal-content">' +
        '        <div class="modal-header">' +
        '        <h4 class="modal-title"><i class="icon fa fa-info"></i>&nbsp;&nbsp;Aguarde...</h4>' +
        '        </div>' +
        '        <div class="modal-body">' +
        '           <p>Aguarde enquanto os pedidos são processados...</p>' +
        '           <div id="ModalContent" class="modal-xn-content"></div>' +
        '        </div>' +
        '        <div class="modal-footer">' +
        '        <button id="ModalOrderClose" class="btn btn-default pull-left" type="button">Fechar</button>' +
        '        <button id="ModalProgressCancel" class="btn btn-warning" type="button" >Cancelar</button>' +
        '        </div>' +
        '    </div>' +
        '    </div>' +
        '</div>';


    var orderCancelationToken = false;
    $("#ConfirmButton").click(function (e) {
        e.preventDefault();
        XNuvem.messageBox.showWarning(
            "Aviso", "Esta operação não pode ser desfeita. Tem certeza que deseja continuar?",
            confirmClick);
    });

    var confirmClick = function () {
        orderCancelationToken = false;
        var allCheckeds = $('.dataTable tbody td input[type="checkbox"]').filter(':checked');
        if (allCheckeds.length == 0) {
            XNuvem.messageBox.showWarning("Aviso", "Nenhum pedido selecionado.");
            return;
        }
        var modal = $(modalProgressHtml);
        $("body").append(modal);
        var modalContent = modal.find("#ModalContent");
        modal.find('#ModalProgressCancel').click(function (e) {
            e.preventDefault();
            orderCancelationToken = true;
            modalContent.prop('scrollTop', 0);
        });
        var dfd = $.Deferred();
        var dfdNext = dfd;
        // Start the pipe chain.  You should be able to do 
        // this anywhere in the program, even
        // at the end,and it should still give the same results.
        dfd.resolve();
        var notifyOrder = function (index) {
            if (index == (allCheckeds.length - 1)) {
                modal.find('.modal-title').html('<i class="icon fa fa-info"></i>&nbsp;&nbsp;Concluído');
            }
        };
        modal.fadeIn('fast', function () {
            allCheckeds.each(function (i, o) {
                if (orderCancelationToken) {
                    return;
                }
                var tr = $(this).parents(tr);
                var data = dt.DataTable().row(tr).data();
                var line = $('<p><i class="fa fa-refresh fa-spin"></i>&nbsp;(' + data.iDocEntry + '/' + data.DocEntry + ') ' + data.CardCode + ' - ' + data.CardName + ' - ' + XNuvem.formatCurrency(data.DocTotal) + '</p>');
                modalContent.append(line);
                modalContent.prop('scrollTop', modalContent.prop('scrollHeight'));
                dfdNext = dfdNext.pipe(function () {
                    return processOrder(i, line, data, notifyOrder);
                });
            });
        });
        modal.find('#ModalOrderClose').click(function (e) {
            e.preventDefault();
            if (dfdNext.state() !== "resolved") {
                XNuvem.statusMessage.showWarning('Aviso', 'O sistema ainda está processando os pedidos.');
                return;
            }
            modal.remove();
            dt.api().draw();
        });
    };

    var processOrder = function (index, line, orderResume, notify) {
        var localDfd = $.Deferred();
        if (orderCancelationToken) {
            line.find('i').removeClass('fa-refresh fa-spin').addClass('fa-close text-red');
            line.append('<br /><span class="text-red">Cancelado pelo usuário</span>');
            notify(index);
            return localDfd.resolve(index);
        }
        if (orderResume.Canceled == 'Y') {
            line.find('i').removeClass('fa-refresh fa-spin').addClass('fa-close text-red');
            line.append('<br /><span class="text-red">Pedido cancelado</span>');
            notify(index);
            return localDfd.resolve(index);
        }
        if (!orderResume.Approved) {
            line.find('i').removeClass('fa-refresh fa-spin').addClass('fa-close text-red');
            line.append('<br /><span class="text-red">Pedido não aprovado</span>');
            notify(index);
            return localDfd.resolve(index);
        }
        if (orderResume.DocEntry) {
            line.find('i').removeClass('fa-refresh fa-spin').addClass('fa-close text-red');
            line.append('<br /><span class="text-red">Pedido já se encontra no SAP</span>');
            notify(index);
            return localDfd.resolve(index);
        }

        var urlAction = $("#ConfirmButton").attr('href');
        $.ajax({
            url: urlAction,
            method: 'post',
            data: orderResume
        }).done(function (d) {
            if (d.IsError) {
                line.find('i').removeClass('fa-refresh fa-spin').addClass('fa-close text-red');
                line.append('<br /><span class="text-red">' + d.Messages.join('. ') + '</span>');
            } else {
                line.find('i').removeClass('fa-refresh fa-spin').addClass('fa-check text-green');
            }
            localDfd.resolve(index);
            notify(index);
        }).fail(function (xhr) {
            line.find('i').removeClass('fa-refresh fa-spin').addClass('fa-close text-red');
            if (xhr.responseText) {
                line.append('<br /><span class="text-red">' + xhr.responseText + '</span>');
            } else {
                line.append('<br /><span class="text-red">Erro ao processar este pedido.</span>');
            }
            localDfd.resolve(index);
            notify(index);
        });
        return localDfd.promise();
    };

    var initStatusColumn = function () {
        $('.dataTable td > a[data-xn-status]').click(function (e) {
            e.preventDefault();
            var tr = $(this).parents(tr);
            var data = dt.DataTable().row(tr).data();

            $.ajax({
                url: statusDetailUrl,
                method: 'post',
                data: { docEntry: data.DocEntry, iDocEntry: data.iDocEntry }
            }).done(function (d) {
                var html = "";
                html += "Cliente: " + d.CardCode + " - " + d.CardName + "<br />";
                html += "Criado em: " + d.CreateDateFmt + "<br />";
                html += "Confirmado em: " + d.OrderDateFmt + "<br />";
                html += "Gerado Nota em: " + d.InvoiceDateFmt + "<br />";
                html += "Valor do Pedido: " + d.DocTotalFmt + "<br />";
                html += "Valor da Nota Fiscal: " + d.InvoiceTotalFmt + "<br />";

                XNuvem.messageBox.showInfo(
                "Informações do Pedido", html);
            }).fail(function (xhr) {
                XNuvem.statusMessage.showWarning('Aviso', 'O sistema não pode retornar as informações do pedido.');
            });
        });
    };
});