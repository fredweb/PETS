/****************************************************************************************
 *
 * Autor: Marvin Mendes
 * Copyright (c) 2017
 * 
/****************************************************************************************/

function parseJsonDate(jsonDate) {
    var offset = new Date().getTimezoneOffset() * 60000;
    var parts = /\/Date\((-?\d+)([+-]\d{2})?(\d{2})?.*/.exec(jsonDate);
    if (parts[2] == undefined) parts[2] = 0;
    if (parts[3] == undefined) parts[3] = 0;
    d = new Date(+parts[1] + offset + parts[2] * 3600000 + parts[3] * 60000);
    date = d.getDate() + 1;
    date = date < 10 ? "0" + date : date;
    mon = d.getMonth() + 1;
    mon = mon < 10 ? "0" + mon : mon;
    year = d.getFullYear();
    return (date + "/" + mon + "/" + year);
};

$(function () {
    var fmtDate = function (d, isDate) {
        d = d.replace('/Date(', '');
        d = d.replace(')/', '');
        var dt = new Date(parseInt(d));
        if (isDate)
            return dt;
        else
            return (dt.getDay() + "/" + dt.getMonth() + "/" + dt.getFullYear());
    };

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

    var dt = $("#cobrancaTable");
    var ajaxUrl = dt.attr("data-xn-url");
    var linkButton = '<a class="btn btn-flat btn-default btn-xs" data-xn-atividades href="#" title="Atividades" data-xn-DocType="{0}" data-xn-DocNum="{1}" data-xn-CardCode="{2}"><i class="fa fa-arrow-right text-orange"></i></a>&nbsp;&nbsp;';
    
    dt.dataTable({
        ajax: {
            url: ajaxUrl,
            type: "POST",
            error: function (xhr, error, thrown) {
                XNuvem.statusMessage.showError("Tabela de dados", xhr.responseText);
            }
        },
        columns: [
            { data: "DocType" },    // 0
            { data: "DocNum" },     // 1
            { data: "DueDate" },    // 2
            { data: "Days" },       // 3
            { data: "CardCode" },   // 4
            { data: "CardName" },   // 5
            { data: "CardFName" },  // 6
            { data: "DocTotal" },   // 7
            { data: "Phone" },      // 8
            { data: "City" },       // 9
            { data: "State" },      // 10
            { data: "SlpName" },    // 11
            { data: "DocStatus" },  // 12
            { data: "Comments" },   // 13
        ],
        columnDefs: [
            { // DocType column
                "orderable": true,
                "targets": [0]
            },
            { // DocNum column
                "orderable": true,
                "targets": [1],
                "render": function (data, type, row, meta) {
                    return linkButton.replace("{0}", row["DocType"]).replace("{1}", row["DocNum"]).replace("{2}", row["CardCode"]) + row["DocNum"];
                }
            },
            { // DueDate
                "orderable": true,
                "visible": true,
                "targets": [2],
                "render": function (data, type, row, meta) {
                    return row["DueDateFmt"];
                }
            },
            { // Days, CardCode, CardName, CardFName
                "orderable": true,
                "targets": [3, 4, 5, 6, 9, 10, 11]
            },
            {
                "orderable": true,
                "targets": [7],
                "render": function (data, type, row, meta) {
                    return row["DocTotalFmt"];
                },
                "className": "text-right"
            },
            {
                "orderable": false,
                "targets": [8, 12, 13]
            }
        ],
        createdRow: function (row, data, index) {
            if (data["Days"] > 0) {
                $(row).addClass('text-red');
            }
        },
        "order": [[2, "desc"]],
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
    }); // dt.dataTable()

    $("#ModalClose").click(function (e) {
        e.preventDefault();
        $("#atividadeModal").hide();
    });

    var refreshModalItens = function () {
        var $content = $("#ModalContent");
        var $modal = $("#atividadeModal");

        var getUrl = $content.attr("data-xn-url");
        var getData = {
            doctype: $modal.attr("data-xn-DocType"),
            docnum: $modal.attr("data-xn-DocNum"),
            cardcode: $modal.attr("data-xn-CardCode")
        };

        $.ajax({
            method: 'GET',
            url: getUrl,
            data: getData
        }).done(function (data) {
            if (data.IsError) {
                XNuvem.statusMessage.showError("Erro!", data.Messages.join('. '));
            } else {
                $content.html('');
                if (data.length == 0) {
                    $content.append("Nenhuma informação encontrada para este documento.");
                }
                for (var i = 0, len = data.length; i < len; i++) {
                    var o = data[i];
                    $content.append('<p>' + o.CreatedBy + ' - ' + parseJsonDate(o.CreatedAt) + ' - ' + o.Comments + '</p>');
                }
                $content.prop('scrollTop', $content.prop('scrollHeight'));
            }
        }).fail(function (xhr) {
            XNuvem.statusMessage.showError("Erro!", "Não foi possível carregar as atividades.");
        });
    };

    dt.on("draw.dt", function (xhre) {
        $('.dataTable td > a[data-xn-atividades]').click(function (e) {
            e.preventDefault();
            var $this = $(this);
            var $modal = $("#atividadeModal");
            $modal.attr("data-xn-DocType", $this.attr("data-xn-DocType"));
            $modal.attr("data-xn-DocNum", $this.attr("data-xn-DocNum"));
            $modal.attr("data-xn-CardCode", $this.attr("data-xn-CardCode"));
            $modal.find("#modalDocument").html($this.attr("data-xn-DocType") + ' - ' + $this.attr("data-xn-DocNum") + ' - ' + $this.attr("data-xn-CardCode"))
            var $content = $("#ModalContent");
            $content.html('');
            refreshModalItens();
            $modal.fadeIn('fast');
        });                
    }); // dt.on("draw.dt")

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

    

    $("#ModalSave").click(function (e) {
        e.preventDefault();

        if ($("#ModalComentario").val().trim() == "") {
            XNuvem.statusMessage.showWarning("Aviso", "É necessário inserir uma informação.");
            return;
        }

        var $this = $(this);
        $this.prop('disabled', true);
        var $modal = $("#atividadeModal");
        var postUrl = $this.attr("data-xn-url");
        var postData = {
            DocType: $modal.attr("data-xn-DocType"),
            DocNum: $modal.attr("data-xn-DocNum"),
            CardCode: $modal.attr("data-xn-CardCode"),
            Comments: $("#ModalComentario").val()
        };

        $.ajax({
            method: 'POST',
            url: postUrl,
            data: postData
        }).done(function (data) {
            $this.prop('disabled', false);
            if (data.IsError) {
                XNuvem.statusMessage.showError("Erro!", data.Messages.join('. '));
            } else {
                refreshModalItens();
                $("#ModalComentario").val("");
                XNuvem.statusMessage.showSuccess("Sucesso!", data.Messages.join('. '));
            }
        }).fail(function (xhr) {
            $this.prop('disabled', false);
            XNuvem.statusMessage.showError("Erro!", "Não foi possível salvar a atividade.");

        });
    });

});