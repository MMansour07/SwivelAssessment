"use strict";
var datatable;

var KTDatatableRecordSelectionDemo = function () {

    var options = {
        data: {
            type: 'remote',
            source: {
                read: {
                    method: 'POST',
                    url: '/user/userlst/',
                    map: function (raw) {
                        var dataSet = raw;
                        if (typeof raw.data !== 'undefined') {
                            dataSet = raw.data;
                        }
                        return dataSet;
                    },
                    timeout: 1000000,
                },
            },
            pageSize: 8,
            serverPaging: true,
            serverFiltering: true,
            serverSorting: true,
            saveState: { cookie: false }
        },
        layout: {
            footer: false
        },
        sortable: true,
        pagination: true
    };
    // basic demo
    var localSelectorDemo = function () {
        options.search = {
            input: $('#kt_datatable_search_query'),
            delay: 1000,
            key: 'generalSearch'
        };

        if (IsSuperAdmin) {
            options.columns = [
                {
                    field: '',
                    title: '#',
                    width: 50,
                    template: function (row, index) {
                        return index + 1;
                    }
                },
                {
                    field: 'FullName',
                    title: 'Full Name',
                    width: 150,
                    template: function (row) {
                        var stateNo = KTUtil.getRandomInt(0, 7);
                        var states = [
                            'success',
                            'primary',
                            'danger',
                            'success',
                            'warning',
                            'dark',
                            'primary',
                            'info'];
                        var state = states[stateNo];

                        return '<div class="d-flex align-items-center">\
								<div class="symbol symbol-40 symbol-'+ state + ' flex-shrink-0">\
									<div class="symbol-label">' + row.FullName.substring(0, 1) + '</div>\
								</div>\
								<div class="ml-2">\
									<div class="text-dark-75 font-weight-bold line-height-sm"></div>\
									<a href="#" class="font-size-sm text-dark-50 text-hover-primary">' + row.FullName + '</a>\
								</div>\
							</div>';
                    }
                },
                {
                    field: 'Email',
                    title: 'Email',
                    width: 150,
                },
                {
                    field: 'UserName',
                    title: 'Username',
                    width: 150,
                },
                {
                    field: 'LastActive',
                    title: 'Last Login Date',
                    sortable: 'desc',
                    width: 150,
                    template: function (row) {
                        if (row.LastActive) {
                            var temp = convertToJavaScriptDate(new Date(parseInt(row.LastActive.replace(/[^0-9]/g, "")))).split(" ");
                            return '<span class="navi-text" style= "float:left; clear:left;">' + temp[0] + '</span>\
                                <span class="navi-text" style= "float:left; clear:left;">' + temp[1] + ' ' + temp[2] + '</span>';
                        }
                        else {
                            return '<span class="navi-text" style= "float:left; clear:left;">NA</span>\
                                <span class="navi-text" style= "float:left; clear:left;">NA</span>';
                        }

                    }
                },
                {
                    field: 'JobsCount',
                    title: '#Jobs',
                    sortable: 'desc',
                    width: 100,
                },
                {
                    field: 'IsAdmin',
                    title: 'Is Admin',
                    template: function (row) {
                        if (row.IsAdmin)
                            return 'Yes';
                        else
                            return 'No';
                    }
                },
                {
                    field: 'CompanyName',
                    title: 'Company',
                },
                {
                    field: 'Phone',
                    title: 'Phone',
                },
                {
                    field: 'Actions',
                    title: 'Actions',
                    width: 120,
                    sortable: false,
                    overflow: 'visible',
                    textAlign: 'left',
                    autoHide: false,
                    template: function (row) {
                        if (!row.IsAdmin) {
                            return '\<a onclick="AddUserToRole(\'' + row.Id + '\')" class="btn btn-sm btn-clean btn-icon mr-2" title="Edit details">\
                            <span class="svg-icon svg-icon-md">\
                            <svg xmlns = "http://www.w3.org/2000/svg" xmlns: xlink = "http://www.w3.org/1999/xlink" width = "24px" height = "24px" viewBox = "0 0 24 24" version = "1.1" >\
                                <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">\
                                    <mask fill="white">\
                                        <use xlink:href="#path-1" />\
                                    </mask>\
                                    <g />\
                                    <path d="M15.6274517,4.55882251 L14.4693753,6.2959371 C13.9280401,5.51296885 13.0239252,5 12,5 C10.3431458,5 9,6.34314575 9,8 L9,10 L14,10 L17,10 L18,10 C19.1045695,10 20,10.8954305 20,12 L20,18 C20,19.1045695 19.1045695,20 18,20 L6,20 C4.8954305,20 4,19.1045695 4,18 L4,12 C4,10.8954305 4.8954305,10 6,10 L7,10 L7,8 C7,5.23857625 9.23857625,3 12,3 C13.4280904,3 14.7163444,3.59871093 15.6274517,4.55882251 Z" fill="#000000" />\
                            </svg ></span >\
	                        </a>\
	                    ';
                        }
                        return '<a onclick="RemoveUserFromRole(\'' + row.Id + '\')" class="btn btn-sm btn-clean btn-icon mr-2" title="Edit details">\
                        <span class="svg-icon svg-icon-md">\
                            <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">\
                                <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">\
                                    <mask fill="white">\
                                        <use xlink:href="#path-1" />\
                                    </mask>\
                                    <path d="M7,10 L7,8 C7,5.23857625 9.23857625,3 12,3 C14.7614237,3 17,5.23857625 17,8 L17,10 L18,10 C19.1045695,10 20,10.8954305 20,12 L20,18 C20,19.1045695 19.1045695,20 18,20 L6,20 C4.8954305,20 4,19.1045695 4,18 L4,12 C4,10.8954305 4.8954305,10 6,10 L7,10 Z M12,5 C10.3431458,5 9,6.34314575 9,8 L9,10 L15,10 L15,8 C15,6.34314575 13.6568542,5 12,5 Z" fill="#000000" />\
                                </g></svg ></span >\
	                        </a>\
                    ';
                    }
                }]
        }
        else {
            options.columns = [
                {
                    field: '',
                    title: '#',
                    width: 50,
                    template: function (row, index) {
                        return index + 1;
                    }
                },
                {
                    field: 'FullName',
                    title: 'Full Name',
                    width: 150,
                    template: function (row) {
                        var stateNo = KTUtil.getRandomInt(0, 7);
                        var states = [
                            'success',
                            'primary',
                            'danger',
                            'success',
                            'warning',
                            'dark',
                            'primary',
                            'info'];
                        var state = states[stateNo];

                        return '<div class="d-flex align-items-center">\
								<div class="symbol symbol-40 symbol-'+ state + ' flex-shrink-0">\
									<div class="symbol-label">' + row.FullName.substring(0, 1) + '</div>\
								</div>\
								<div class="ml-2">\
									<div class="text-dark-75 font-weight-bold line-height-sm"></div>\
									<a href="#" class="font-size-sm text-dark-50 text-hover-primary">' + row.FullName + '</a>\
								</div>\
							</div>';
                    }
                },
                {
                    field: 'Email',
                    title: 'Email',
                    width: 150,
                },
                {
                    field: 'UserName',
                    title: 'Username',
                    width: 150,
                },
                {
                    field: 'LastActive',
                    title: 'Last Login Date',
                    sortable: 'desc',
                    width: 150,
                    template: function (row) {
                        if (row.LastActive) {
                            var temp = convertToJavaScriptDate(new Date(parseInt(row.LastActive.replace(/[^0-9]/g, "")))).split(" ");
                            return '<span class="navi-text" style= "float:left; clear:left;">' + temp[0] + '</span>\
                                <span class="navi-text" style= "float:left; clear:left;">' + temp[1] + ' ' + temp[2] + '</span>';
                        }
                        else {
                            return '<span class="navi-text" style= "float:left; clear:left;">NA</span>\
                                <span class="navi-text" style= "float:left; clear:left;">NA</span>';
                        }

                    }
                },
                {
                    field: 'JobsCount',
                    title: '#Jobs',
                    sortable: 'desc',
                    width: 100,
                },
                {
                    field: 'IsAdmin',
                    title: 'Is Admin',
                    template: function (row) {
                        if (row.IsAdmin)
                            return 'Yes';
                        else
                            return 'No';
                    }
                },
                {
                    field: 'CompanyName',
                    title: 'Company',
                },
                {
                    field: 'Phone',
                    title: 'Phone',
                }]
        }

        datatable = $('#kt_datatable').KTDatatable(options);

        $('#kt_datatable_search_status').on('change', function () {
            datatable.search($(this).val(), 'status');
        });

        $('#kt_datatable_search_status, #kt_datatable_search_type').selectpicker();

        datatable.on('datatable-on-check datatable-on-uncheck', function (e) {
            var checkedNodes = datatable.rows('.datatable-row-active').nodes();
            var count = checkedNodes.length;
        });

        datatable.on('click', '[data-record-id]', function () {
            localStorage.clear();
        });


    };

    return {
        // public functions
        init: function () {
            localSelectorDemo();
        },
    };
}();
jQuery(document).ready(function () {
    localStorage.clear();
    KTDatatableRecordSelectionDemo.init();
});

function AddUserToRole(Id) {
    Swal.fire({
        title: "Are you sure you want add admin role to this user ?",
        text: "Please Confirm!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Yes!",
        cancelButtonText: "No!",
        reverseButtons: true
    }).then(function (result) {
        if (result.value) {
            KTApp.blockPage();
            $.ajax({
                url: "/user/AddUserToRole?userId=" + Id,
                type: "GET",
                dataType: 'json',
                contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                success: function (response) {
                    KTApp.unblockPage();
                    if (response.Success) {
                        datatable.reload();
                        KTUtil.scrollTop();
                        toastr.options = {
                            "closeButton": false,
                            "debug": false,
                            "newestOnTop": true,
                            "progressBar": false,
                            "positionClass": "toast-bottom-left",
                            "preventDuplicates": false,
                            "onclick": null,
                            "showDuration": "300",
                            "hideDuration": "1000",
                            "timeOut": "5000",
                            "extendedTimeOut": "1000",
                            "showEasing": "swing",
                            "hideEasing": "linear",
                            "showMethod": "fadeIn",
                            "hideMethod": "fadeOut"
                        };

                        toastr.success("Process has been executed successfully!");
                    }
                    else {
                        toastr.options = {
                            "closeButton": false,
                            "debug": false,
                            "newestOnTop": true,
                            "progressBar": false,
                            "positionClass": "toast-bottom-left",
                            "preventDuplicates": false,
                            "onclick": null,
                            "showDuration": "300",
                            "hideDuration": "1000",
                            "timeOut": "5000",
                            "extendedTimeOut": "1000",
                            "showEasing": "swing",
                            "hideEasing": "linear",
                            "showMethod": "fadeIn",
                            "hideMethod": "fadeOut"
                        };

                        toastr.error("Something went wrong!");
                    }
                }
            });
        }
    });
}

function RemoveUserFromRole(Id) {
    Swal.fire({
        title: "Are you sure you want remove admin role from this user ?",
        text: "Please Confirm!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Yes!",
        cancelButtonText: "No!",
        reverseButtons: true
    }).then(function (result) {
        if (result.value) {
            KTApp.blockPage();
            $.ajax({
                url: "/user/RemoveUserFromRole?userId=" + Id,
                type: "GET",
                dataType: 'json',
                contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                success: function (response) {
                    KTApp.unblockPage();
                    if (response.Success) {
                        datatable.reload();
                        KTUtil.scrollTop();
                        toastr.options = {
                            "closeButton": false,
                            "debug": false,
                            "newestOnTop": true,
                            "progressBar": false,
                            "positionClass": "toast-bottom-left",
                            "preventDuplicates": false,
                            "onclick": null,
                            "showDuration": "300",
                            "hideDuration": "1000",
                            "timeOut": "5000",
                            "extendedTimeOut": "1000",
                            "showEasing": "swing",
                            "hideEasing": "linear",
                            "showMethod": "fadeIn",
                            "hideMethod": "fadeOut"
                        };

                        toastr.success("Process has been executed successfully!");
                    }
                    else {
                        toastr.options = {
                            "closeButton": false,
                            "debug": false,
                            "newestOnTop": true,
                            "progressBar": false,
                            "positionClass": "toast-bottom-left",
                            "preventDuplicates": false,
                            "onclick": null,
                            "showDuration": "300",
                            "hideDuration": "1000",
                            "timeOut": "5000",
                            "extendedTimeOut": "1000",
                            "showEasing": "swing",
                            "hideEasing": "linear",
                            "showMethod": "fadeIn",
                            "hideMethod": "fadeOut"
                        };

                        toastr.error("Something went wrong!");
                    }
                }
            });
        }
    });
}