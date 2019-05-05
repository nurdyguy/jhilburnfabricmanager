
var typeDelay = 100;
var delayPause = false;
$(document).ready(function ()
{
	$('.sortable').on('click', function ()
	{
		var $this = $(this);
		var sortBy = $this.data('sort');
		var sortDir = '';

		if ($this.find('span.sort').hasClass('fa-chevron-up'))
		{
			sortDir = 'desc';			
			$this.closest('.row').find('span.sort').removeClass('fa-chevron-up fa-chevron-down');
			$this.find('span.sort').addClass('fa-chevron-down');
		}
		else
		{
			sortDir = 'asc';
			$this.closest('.row').find('span.sort').removeClass('fa-chevron-up fa-chevron-down');
			$this.find('span.sort').addClass('fa-chevron-up');			
		}

		$('#pagedListProps').attr('data-sort-by', sortBy).attr('data-sort-dir', sortDir);

		GoToPage(1);
	});

	$('.pagination').on('click', '.page-item', function ()
	{
		var $this = $(this);
		if ($this.hasClass('disabled') || $this.hasClass('active'))
			return;

		GoToPage($this.find('a.page-link').attr('data-page-num'));
	});

	$('#searchInput').on('keyup', function (e)
	{
		if (delayPause)
			return;

		delayPause = true;
		setTimeout(function () { delayPause = false; }, typeDelay);

		GoToPage(1);
	});

	$('#fabricTable').find('.tableBody').on('click', '.row', function (e)
	{
		var $this = $(this);
		if ($(e.target).hasClass('fabricDelete'))
			DeleteFabric($this.data('id'));
		else
			ShowFabricDetail($this.data('id'))
	});

	$('#fabricDetailModal').on('click', 'button.preview', function ()
	{
		var url = $('#imgInput').find('input').val();
		$('#imgPreview').find('img').attr('src', url);
	});

	$('#fabricDetailModal').on('click', '#saveDetail', function ()
	{
		SaveFabricDetail();
	});

	$('#createFabric').on('click', function () {
		ShowFabricDetail(0);
	});
});

function GetList()
{
	ShowWaiting();

	var $props = $('#pagedListProps');

	var request = {
		Page: $props.attr('data-to-page'),
		PerPage: $props.data('per-page'),
		Search: BuildSearchString(),
		SortBy: $props.data('sort-by'),
		SortDir: $props.data('sort-dir')
	};

	$.ajax(
	{
		url: '/fabricManager/getfabrics',
		type: "POST",
		contentType: 'application/json;',
		data: JSON.stringify(request),
		success: function (result) 
		{
			$('#fabricTable').find('.tableBody').html(result);
			UpdatePaging();
		},
		error: function (e) 
		{

		},
		complete: function () 
		{
			HideWaiting();
		}
	});

}

function GoToPage(pageNum)
{
	$props = $('#pagedListProps');
	var toPage = 0;
	var currPage = parseInt($props.data('page'), 10);
	var perPage = parseInt($props.data('per-page'), 10);
	var totalCount = parseInt($props.attr('data-total-count'), 10);

	switch (pageNum)
	{
		case 'first':
			toPage = 1;
			break;
		case 'prev':
			toPage = currPage - 1;
			break;
		case 'next':
			toPage = currPage + 1;
			break;
		case 'last':
			toPage = Math.ceil((totalCount + .0) / perPage);
			break;
		case 'farForward':
			toPage = 1;
			break;
		case 'farBack':
			toPage = 1;
			break;
		default:
			toPage = parseInt(pageNum);
	}

	$props.attr('data-to-page', toPage);
	GetList();
}

function BuildSearchString()
{
	var input = $('#searchInput').val();
	var search = '';
	if (input && input.trim() != '')
		search = 'description_like=' + input;
	return search;
}

function UpdatePaging()
{
	$props = $('#pagedListProps');
	$pageButtons = $('.pagination').find('.page-item');

	var currPage = parseInt($props.data('page'), 10);
	var perPage = parseInt($props.data('per-page'), 10);
	var totalCount = parseInt($props.attr('data-total-count'), 10);
	var totalPages = Math.ceil((totalCount + .0) / perPage);

	if (currPage == 1)
	{
		$pageButtons.filter('.page-first').addClass('disabled');
		$pageButtons.filter('.page-prev').addClass('disabled');
	}
	else
	{
		$pageButtons.filter('.page-first').removeClass('disabled');
		$pageButtons.filter('.page-prev').removeClass('disabled');
	}

	var $pageNumberButtons = $pageButtons.filter('.page-num');

	for (var i = totalPages + 1; i <= $pageNumberButtons.length; i++)
	{
		$pageNumberButtons.eq(i - 1).remove();
	}

	for (var i = $pageNumberButtons.length + 1; i <= totalPages; i++)
	{
		var $newButton = $('#buttonTemplate').clone();
		$newButton.find('a.page-link').attr('data-page-num', i).html(i);
		$pageButtons.filter('.page-next').before($newButton.html());
	}
	
	$pageNumberButtons.removeClass('active').eq(currPage - 1).addClass('active');

	if (currPage == totalPages)
	{
		$pageButtons.filter('.page-next').addClass('disabled');
		$pageButtons.filter('.page-last').addClass('disabled');
	}
	else
	{
		$pageButtons.filter('.page-next').removeClass('disabled');
		$pageButtons.filter('.page-last').removeClass('disabled');
	}
}

function ShowFabricDetail(fabricId)
{
	ShowWaiting();
	$.ajax(
	{
		url: '/fabricManager/GetDetail/' + fabricId,
		type: "GET",
		contentType: 'application/json;',
		success: function (result) 
		{			
			$('#fabricDetailModal').modal('show').find('.modal-content').html(result);		
		},
		error: function (e)
		{

		},
		complete: function () 
		{
			HideWaiting();
		}
	});
}

function ValidateInputs(isCreate)
{
	var valid = true;
	var intRegex = /^\d+$/;
	var floatRegex = /^-?\d*(\.\d+)?$/;
	$('#fabricDetailModal').find('.is-invalid').removeClass('is-invalid');

	var $skuInput = $('#skuInput').find('input');
	if ($skuInput.val().trim() == '')
	{
		$skuInput.addClass('is-invalid');
		valid = false;
	}

	var $descriptionInput = $('#descriptionInput').find('input');
	if ($descriptionInput.val().trim() == '')
	{
		$descriptionInput.addClass('is-invalid');
		valid = false;
	}

	var $priceInput = $('#priceInput').find('input');
	if ($priceInput.val().trim() == '' || !floatRegex.test($priceInput.val()))
	{
		$priceInput.addClass('is-invalid');
		valid = false;
	}

	var $inventoryInput = $('#inventoryInput').find('input');
	if ($inventoryInput.val().trim() == '' || !intRegex.test($inventoryInput.val()))
	{
		$inventoryInput.addClass('is-invalid');
		valid = false;
	}

	var $imgInput = $('#imgInput').find('input');
	if (!isCreate && $imgInput.val().trim() == '')
	{
		$imgInput.addClass('is-invalid');
		valid = false;
	}

	return valid;
}

function SaveFabricDetail()
{
	if (!ValidateInputs($('#fabricDetailModal').find('.form-group').data('is-create')))
		return;

	var fab = {
		Active: $('#activeInput').find('input').is(':checked'),
		Category: $('#categoryInput').find('select').val(),
		Description: $('#descriptionInput').find('input').val(),
		Id: $('#idInput').find('input').val(),
		ImgUrl: $('#imgInput').find('input').val(),
		Inventory: $('#inventoryInput').find('input').val(),
		Price: $('#priceInput').find('input').val(),
		Sku: $('#skuInput').find('input').val(),
	}

	ShowWaiting();
	$.ajax(
	{
		url: '/fabricManager/SaveDetail/',
		type: "Post",
		contentType: 'application/json;',
		data: JSON.stringify(fab),
		success: function (result) 
		{
			$('#fabricDetailModal').modal('hide')
			GoToPage(1);
		},
		error: function (e) 
		{

		},
		complete: function () 
		{
			HideWaiting();
		}
	});

}

function DeleteFabric(fabricId)
{
	if (!confirm('Are you sure you want to delete this fabric?'))
		return;

	ShowWaiting();
	$.ajax(
	{
		url: '/fabricManager/Delete/' + fabricId,
		type: "DELETE",
		contentType: 'application/json;',
		success: function (result) 
		{			
			GoToPage(1);
		},
		error: function (e) 
		{

		},
		complete: function ()
		{
			HideWaiting();
		}
	});
}