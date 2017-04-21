﻿
function dashboard(idGram, idLegen, fData) {
	var barColor = 'steelblue';	
	function segColor(c) { return { luuhs: "#969696", daxuly: "#393b79", dangxuly: "#17becf", trinhky: "#9467bd", quahan: "#d62728" }[c]; }
    // compute total 
	
	fData.forEach(function (d) {
	    d.total = parseInt(d.thongke.luuhs) + parseInt(d.thongke.daxuly)
            + parseInt(d.thongke.dangxuly) + parseInt(d.thongke.trinhky) + parseInt(d.thongke.quahan);	
	});

	// function to handle histogram.
	function histoGram(fD) {
		var count = 0;
		fD.forEach(function (d) { count++; });
		var _hWidth;
		if (count < 10) {
		    _hWidth = count * 70;
		} else {
		    _hWidth = count * 40;
		}

		var bWidth = 30;

		var hG = {}, hGDim = { t: 30, r: 10, b: 150, l: 60 };
		hGDim.w = _hWidth - hGDim.l - hGDim.r,
		hGDim.h = 400 - hGDim.t - hGDim.b;
		
		//create svg for histogram.
		var hGsvg = d3.select(idGram).append("svg")
			.attr("width", hGDim.w + hGDim.l + hGDim.r)
			.attr("height", hGDim.h + hGDim.t + hGDim.b).append("g")
			.attr("transform", "translate(" + hGDim.l + "," + hGDim.t + ")");

		// create function for x-axis mapping.
		var x = d3.scale.ordinal().rangeRoundBands([0, hGDim.w], 0.1)
				.domain(fD.map(function (d) { return d[0]; }));

		// Add x-axis to the histogram svg.
		hGsvg.append("g").attr("class", "x axis")
			.attr("transform", "translate(0," + hGDim.h + ")")
			//.call(d3.svg.axis().scale(x).orient("bottom"));
			.call(d3.svg.axis().scale(x).orient("bottom"))
			.selectAll("text")
			.style("text-anchor", "end")
			.attr("dx", "-.8em")
			.attr("dy", ".15em")
			.attr("transform", function (d) { return "rotate(-45)" });


		// quay nghieng text
		//hGsvg.selectAll(".axis text")
		//.attr("transform", "translate(" + hGDim.h * -1 + "," + hGDim.w + ")rotate(-45)");
		//.style("text-anchor", "end").attr("transform", function(d) { return "rotate(-45)" });


		// Create function for y-axis map.
		var y = d3.scale.linear().range([hGDim.h, 0])
				.domain([0, d3.max(fD, function (d) { return d[1]; })]);

		// Create bars for histogram to contain rectangles and freq labels.
		var bars = hGsvg.selectAll(".bar").data(fD).enter()
				.append("g").attr("class", "bar");

	    //create the rectangles.
		
		
		bars.append("rect")
			.attr("x", function (d) { return x(d[0]); })
			.attr("y", function (d) { return y(d[1]); })
			.attr("width", x.rangeBand())
			//.attr("width", bWidth)
			.attr("height", function (d) { return hGDim.h - y(d[1]); })
			.attr('fill', barColor)
			.on("mouseover", mouseover)// mouseover is defined below.
			.on("mouseout", mouseout);// mouseout is defined below.

		//Create the frequency labels above the rectangles.
		bars.append("text").text(function (d) { return d3.format(",")(d[1]) })
			//.attr("x", function(d) { return x(d[0])+x.rangeBand()/2; })
			.attr("x", function (d) { return x(d[0]) + x.rangeBand() / 2; })
			.attr("y", function (d) { return y(d[1]) - 5; })
			.attr("text-anchor", "middle");

		function mouseover(d) {  // utility function to be called on mouseover.
			// filter for selected ten.
			var st = fData.filter(function (s) { return s.ten == d[0]; })[0],
				nD = d3.keys(st.thongke).map(function (s) { return { type: s, thongke: st.thongke[s] }; });

			// call update functions of pie-chart and legend.    
			pC.update(nD);
			leg.update(nD, st.id);			
		}

		function mouseout(d) {    // utility function to be called on mouseout.
			// reset the pie-chart and legend.    
			pC.update(tF);
			leg.update(tF, 0);
		}

		// create function to update the bars. This will be used by pie-chart.
		hG.update = function (nD, color) {
			// update the domain of the y-axis map to reflect change in frequencies.
			y.domain([0, d3.max(nD, function (d) { return d[1]; })]);

			// Attach the new data to the bars.
			var bars = hGsvg.selectAll(".bar").data(nD);
			//console.log(hGDim.h);
			//console.log(nD);
			// transition the height and color of rectangles.
			bars.select("rect").transition().duration(500)
				.attr("y", function (d) { return y(d[1]); })
				.attr("height", function (d) { return hGDim.h - y(d[1]); })
				.attr("fill", color);

			// transition the frequency labels location and change value.
			bars.select("text").transition().duration(500)
				.text(function (d) { return d3.format(",")(d[1]) })
				.attr("y", function (d) { return y(d[1]) - 5; });
		}
		return hG;
	}

	// function to handle pieChart.
	function pieChart(pD) {
		var pC = {}, pieDim = { w: 250, h: 250 };
		pieDim.r = Math.min(pieDim.w, pieDim.h) / 2;

		// create svg for pie chart.
		var piesvg = d3.select(idLegen).append("svg")
			.attr("width", pieDim.w).attr("height", pieDim.h).append("g")
			.attr("transform", "translate(" + pieDim.w / 2 + "," + pieDim.h / 2 + ")");

		// create function to draw the arcs of the pie slices.
		var arc = d3.svg.arc().outerRadius(pieDim.r - 10).innerRadius(0);

		// create a function to compute the pie slice angles.
		var pie = d3.layout.pie().sort(null).value(function (d) { return d.thongke; });

		// Draw the pie slices.
		piesvg.selectAll("path").data(pie(pD)).enter().append("path").attr("d", arc)
			.each(function (d) { this._current = d; })
			.style("fill", function (d) { return segColor(d.data.type); })
			.on("mouseover", mouseover).on("mouseout", mouseout);

		// create function to update pie-chart. This will be used by histogram.
		pC.update = function (nD) {
			piesvg.selectAll("path").data(pie(nD)).transition().duration(500)
				.attrTween("d", arcTween);
		}
		// Utility function to be called on mouseover a pie slice.
		function mouseover(d) {
			// call the update function of histogram with new data.
			hG.update(fData.map(function (v) {
				return [v.ten, v.thongke[d.data.type]];
			}), segColor(d.data.type));
		}
		//Utility function to be called on mouseout a pie slice.
		function mouseout(d) {
			// call the update function of histogram with all data.
			hG.update(fData.map(function (v) {
				return [v.ten, v.total];
			}), barColor);
		}
		// Animating the pie-slice requiring a custom function which specifies
		// how the intermediate paths should be drawn.
		function arcTween(a) {
			var i = d3.interpolate(this._current, a);
			this._current = i(0);
			return function (t) { return arc(i(t)); };
		}
		return pC;
	}

	// function to handle legend.
	function legend(lD, ten) {
		var leg = {};
		
		// create table for legend.
		var legend = d3.select(idLegen).append("table").attr('class', 'legend');
		
		function GetHoten(_id) {
			if (_id == 0) {
				return "Tổng cộng";
			} else {
				var _hoten = "";
				fData.forEach(function (d) {
					if (_id == d.id) {
						_hoten = d.ten;
					}
				});
				return _hoten;
			}
		}
		var hoten = GetHoten(ten);
		var dheader = [{ tieude: ten, id: 0 }];
		var header = legend.append("thead").selectAll("th").data(dheader).enter().append("tr");
		header.append("td").attr('colspan', '4').attr('class', 'clHeader').text(function (d) { return hoten });

		// create one row per segment.
		var tr = legend.append("tbody").selectAll("tr").data(lD).enter().append("tr");

		// create the first column for each segment.
		tr.append("td").append("svg").attr("width", '16').attr("height", '16').append("rect")
			.attr("width", '16').attr("height", '16')
			.attr("fill", function (d) { return segColor(d.type); });

		// create the second column for each segment.
		tr.append("td").text(function (d) { return getLegenType(d.type); });

		// create the third column for each segment.
		tr.append("td").attr("class", 'legendFreq')
			.text(function (d) { return d3.format(",")(d.thongke); });

		// create the fourth column for each segment.
		tr.append("td").attr("class", 'legendPerc')
			.text(function (d) { return getLegend(d, lD); });

		// Utility function to be used to update the legend.
		leg.update = function (nD, id) {
			var hD = [{ id: id, ten: " " }];
			var h = legend.select("thead").selectAll("tr").data(hD);

			var hoten = GetHoten(id);
			h.select(".clHeader").text(hoten);

			// update the data attached to the row elements.
			var l = legend.select("tbody").selectAll("tr").data(nD);

			// update the frequencies.
			l.select(".legendFreq").text(function (d) { return d3.format(",")(d.thongke); });

			// update the percentage column.
			l.select(".legendPerc").text(function (d) { return getLegend(d, nD); });
		}

		function getLegend(d, aD) { // Utility function to compute percentage.
			return d3.format("%")(d.thongke / d3.sum(aD.map(function (v) { return v.thongke; })));
		}

		function getLegenType(d) {
		    var strloai;
		    switch (d) {
		        case "luuhs":
		            strloai = "Lưu hồ sơ";
		            break;
		        case "daxuly":
		            strloai = "Đã xử lý";
		            break;
		        case "dangxuly":
		            strloai = "Đang xử lý";
		            break;
		        case "trinhky":
		            strloai = "Đang trình ký";
		            break;
		        case "quahan":
		            strloai = "Đã quá hạn";
		            break;
		        default:
		            strloai = d;
		            break;
		    }
		    return strloai;
		}

		return leg;
	}

	// calculate total frequency by segment for all state.
	var tF = ['luuhs', 'daxuly', 'dangxuly', 'trinhky', 'quahan'].map(function (d) {
		return { type: d, thongke: d3.sum(fData.map(function (t) { return t.thongke[d]; })) };
	});

	// calculate total frequency by state for all segment.
	var sF = fData.map(function (d) { return [d.ten, d.total]; });

	var hG = histoGram(sF), // create the histogram.
		pC = pieChart(tF), // create the pie-chart.
		leg = legend(tF, 0);  // create the legend.
}
