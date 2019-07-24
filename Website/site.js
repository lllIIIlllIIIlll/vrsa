var username;
var db = firebase.database();
var events = db.ref('Events');

// Ui Functionality
function AddOrUpdateEventRow(id, event) {

	var row = $('<tr>');
	row.append($('<td>' + event.Name + '</td>'));
	row.append($('<td>' + event.Type + '</td>'));
	row.append($('<td>' + event.Business + '</td>'));
	row.append($('<td style="text-align: center;"><img data-id="' + id + '" class="remove-event" src="remove.png" class="add-event" /></td>'));

	$('#events').append(row);

	RemoveEventRowBinding();
}

function RemoveEventRowBinding() {
	$('.remove-event').unbind();
	$('.remove-event').click(function() {

		var id = $(this).data('id');
		events.child(id).remove();
	});
}

function AddOrUpdateBusinessRow(name, business) {

	var row = $('<tr>');
	row.append($('<td>' + name + '</td>'));

	var ss = $('<td>');
	var sale = $('<div class="sale" style="width: 600px;"><input id="' + name + '" data-type="Sale" min="0" max="100" step="1" value="' + business.Sale + '" width="300px" type="range" /></div>');
	var supply = $('<div style="width: 600px;"><input id="' + name + '" data-type="Supply" min="0" max="100" step="1" value="' + business.Supply + '" width="300px" type="range" /></div>');
	ss.append(sale);
	ss.append(supply);

	row.append(ss)
	row.append($('<td style="text-align: center;"><img data-id="' + name + '" class="remove-business" src="remove.png" class="add-business" /></td>'));

	$('#businesses').append(row);

	RemoveBusinessRowBinding();
}

function RemoveBusinessRowBinding() {
	$('.remove-business').unbind();
	$('.remove-business').click(function() {

		var id = $(this).data('id');
		var businessRef = db.ref('Users/' + username + '/Businesses');
		businessRef.child(id).remove();
	});
}

function InterfaceBindings() {

	RangeInputs();

	ReBindClick('.add-event', function() {
		var name = $('#username').text();
		var type = $("#event-type").val();
		var business = $("#event-business").val();
		AddEvent(name, type, business);
	});

	ReBindClick('.add-business', function() {
		var name = $('#business').val();
		AddBusiness(name);
	});
	
	ReBindClick('#authenticate', (function() {
		var password = $('#password-input').val();
		var _username = $('#username-input').val();
		if (_username === '' || password === '') {
			alert('Username and password are required');
		} else {

			// TODeploy pasword check
		
			SetCookie('username', _username);

			Authenticated(_username);

			$('#username-modal').modal('toggle');
		}
	}));

	ReBindClick('#bypass', function() {
		var username = $('#bypass-username').val();
		Authenticated(username);
	});

	ReBindClick('#logout', function() {
		DeleteCookie('username');
		$('#site').addClass('hide');
		Authentication();
	});

}

function RangeInputs(fill) {

	$(document).ready(function() {
		$('input[type="range"]').each(function() {
			var ele = $(this);
			var id = ele.attr('id');
			var type = ele.data('type');
			ele.rangeslider({

				// Feature detection the default is `true`.
				// Set this to `false` if you want to use
				// the polyfill also in Browsers which support
				// the native <input type="range"> element.
				polyfill: false,
			
				// Default CSS classes
				rangeClass: 'rangeslider',
				disabledClass: 'rangeslider--disabled',
				horizontalClass: 'rangeslider--horizontal',
				verticalClass: 'rangeslider--vertical',
				fillClass: 'rangeslider__fill',
				handleClass: 'rangeslider__handle',
			
				// Callback function
				onInit: function() {},
			
				// Callback function
				onSlide: function(position, value) {},
			
				// Callback function
				onSlideEnd: function(position, percentage) {
					UpdateBusiness(id, type, percentage);
				}
			});
		})
	});
}

function ReBindClick(control, func) {
	var _control = $(control);
	_control.unbind();
	_control.click(func);
}

// Data

function DataBindings(username) {

	events.on("value", function(data) {
	
		$('#events').empty();
		
		data.forEach(function(event) {
			AddOrUpdateEventRow(event.key, event.val());
		});
		
	});

	var businessRef = db.ref('Users/' + username + '/Businesses');
	businessRef.on("value", function(data) {
	
		$('#businesses').empty();

		data.forEach(function(business) {
			AddOrUpdateBusinessRow(business.key, business.val());
		});

		RangeInputs();
	});

}

function AddBusiness(name) {
	var businessRef = db.ref('Users/' + username + '/Businesses/' + name);
	businessRef.set({
		Sale: 0,
		Supply: 0
	})
}

function AddEvent(name, type, business) {
	events.push({
		Name: name + '1',
		Type: type,
		Business: business
	});
}

function UpdateBusiness(name, type, value) {
	db.ref('Users/' + username + '/Businesses/' + name + '/' + type).set(value);
}

// Cookies

function SetCookie(name,value,days) {
	var expires = "";
	if (days) {
		var date = new Date();
		date.setTime(date.getTime() + (days*24*60*60*1000));
		expires = "; expires=" + date.toUTCString();
	}
	document.cookie = name + "=" + (value || "")  + expires + "; path=/";
}

function GetCookie(name) {
	var nameEQ = name + "=";
	var ca = document.cookie.split(';');
	for(var i=0;i < ca.length;i++) {
		var c = ca[i];
		while (c.charAt(0)==' ') c = c.substring(1,c.length);
		if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length,c.length);
	}
	return null;
}

function DeleteCookie(name) {   
	document.cookie = name+'=; Max-Age=-99999999;';  
}
			
// Helpers

function Authentication() {

	var username = GetCookie('username');
	if (username === null) {
		$('#username-modal').modal({
			keyboard: false,
			backdrop: 'static'
		});
	} else {
		Authenticated(username);
	}
}

function Authenticated(_username) {
	username = _username;
	$('.username').text(_username);
	$('#site').removeClass('hide');
	DataBindings(username);
}

$(document).ready(function() {
	InterfaceBindings();
	Authentication();
});