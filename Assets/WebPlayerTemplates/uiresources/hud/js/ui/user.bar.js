ui._userBarTemplate = function()
{
	return $('<div class="user-bar"><div class="user-name-bar">User name</div><div class="main-circle"></div><div class="inner-circle"></div>'+
	'<div class="minerals-bar"></div><div class="mineral-icon"></div><div class="gas-bar"></div>'+
	'<div class="gas-icon"></div><div class="minerals-amount">1,087,009</div>'+
	'<div class="gas-amount">879,700</div></div>');
}


// Attaching user bar to the scene
ui.attachUserBar = function()
{
	var tpl = ui._userBarTemplate();
	tpl.appendTo("#hud");

	var circle = tpl.find(".main-circle")[0];
	var animate = function()
	{
		move(circle).ease('out').duration(2000).rotate(30).end(function(){
			setTimeout(function(){
				move(circle).rotate(0).duration(2000).ease('out').end(function(){
					animate();
				});
			},1)
		});	
	}
	setTimeout(function(){
		animate();	
	},1);
}