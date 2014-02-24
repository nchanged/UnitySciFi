window.ui = window.ui || {};
ui.animate = function(element,options){
	setTimeout(function() {
		this.o = this.o || {};

		move(this.el)
			.duration(300)
			.ease('out')
			.set("opacity", this.o.opacity ? this.o.opacity : 1)
			.y(this.o.y ? this.o.y : 0)
			.scale(this.o.scale ? this.o.scale : 1)
			.rotate(this.o.rotate ? this.o.rotate : 0)
			.end();
	}.bind({o : options, el : element}), 1);
}

ui.getBottomBar = function()
{
	return $(".bottom-bar");
}
ui.showUnitButtons = function(options) {
	var getTemplate = function() {
		return $('<div class="button unit-button"><div class="inner-circle"></div></div>');
	}

	var bottomBar = ui.getBottomBar();

	// Removing existing buttons
	bottomBar.find('.unit-button').remove();

	var unit_id = options.unit_id;
	// Showing research button
	if (options.research) {
		var research = getTemplate();
		research.find('.inner-circle').addClass("btn-research");
		move(research[0]).y(100).set("opacity", 0).rotate(90).scale(0.5).end();
		research.appendTo(bottomBar);

		ui.animate(research[0]);

		var research = getTemplate();
		research.find('.inner-circle').addClass("btn-research");
		move(research[0]).y(100).set("opacity", 0).rotate(90).scale(0.5).end();
		research.appendTo(bottomBar);

		ui.animate(research[0]);
		
	}
}

ui.hideUnitButtons = function()
{
	var bottomBar = ui.getBottomBar();
	bottomBar.find(".unit-button").each(function(){
		ui.animate(this,{
			opacity : 0, scale : 0.3, rotate : 90, y :100
		})
	})
}