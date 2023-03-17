(function($) {
	$.widget("ui.checkList", {	
		options: {
			listItems : [],
			name : '',
			selectedItems: [],
			effect: 'blink',
			onChange: {},
			objTable: '',
			icount: 0
		},
		
		_create: function() {
			var self = this, o = self.options, el = self.element;

			// generate outer div
			var container = $('<div/>').addClass('checkList-container');
			var selected = $('<div/>').addClass('selectedcheckList');
			var checkList = $('<div/>').addClass('checkList');
			container.append(checkList);
			container.append(selected);
			// generate toolbar
			o.selectedTitle = $('<label/>').addClass('selectedTitle').text('Selected');
			var toolbar = $('<div/>').addClass('toolbar');
			var selectedToolbar = $('<div/>').addClass('toolbar');
			selectedToolbar.append($('<div/>').addClass('selected-title-container').append(o.selectedTitle));
			selected.append(selectedToolbar);
			var chkAllContainer = $('<div/>').addClass('check-all');
			var chkAll = $('<input/>').attr('type','checkbox').addClass('chkAll').click(function(){

				if($(this).prop('checked')){
					o.objTable.find('.chk:visible').prop('checked', true);
				}else{
					o.objTable.find('.chk:visible').prop('checked',false);
				}
				self._selChange();

			});
			var txtfilter = $('<input/>').attr('type','text').attr('placeholder','Filter search').addClass('txtFilter').keyup(function(){
				self._filter($(this).val());
			});
			chkAllContainer.append(chkAll);
			toolbar.append(chkAllContainer);
			toolbar.append($('<div/>').addClass('filterbox col-sm-11').append(txtfilter));

			// generate list table object

			var wrapper1 = $('<div/>').addClass('checklist-wrapper col-md-12');
			var wrapper2 = $('<div/>').addClass('checklist-wrapper col-md-12');
			o.selectedTable = $('<table/>').addClass('selectedChecklist-table');
			o.objTable = $('<table/>').addClass('checklist-table');
			wrapper1.append(o.objTable);
			wrapper2.append(o.selectedTable);
			checkList.append(toolbar);
			checkList.append(wrapper1);
			selected.append(wrapper2);
			el.append(container);

			self.loadList();
		},

		_addItem: function(listItem){
			var self = this, o = self.options, el = self.element;

			var itemId = 'itm' + (o.icount++);	// generate item id
			var itm = $('<tr/>');
			var chk = $('<input/>').attr('type','checkbox').attr('id',itemId)
					.addClass('chk')
					.attr('name',o.name)
					.attr('value',listItem.value)
					.attr('data-text',listItem.text)
					.attr('data-value',listItem.value);
			
			itm.append($('<td/>').append(chk));
			var label = $('<label/>').attr('for',itemId).text(listItem.text);
			itm.append($('<td/>').append(label));
			o.objTable.append(itm);

			// bind selection-change
			el.delegate('.chk','click', function(){self._selChange()});
		},

		loadList: function(){
			var self = this, o = self.options, el = self.element;

			o.objTable.empty();
			$.each(o.listItems,function(){
				console.log(JSON.stringify(this));
				self._addItem(this);
			});
		},

		_selChange: function(){
			var self = this, o = self.options, el = self.element;

			// empty selection
			o.selectedItems = [];
			var count = 0;
			// scan elements, find checked ones
			o.objTable.find('.chk').each(function(){	
				var state = $(this).prop('checked');

				if($(this).prop('checked') == true){
					o.selectedItems.push({
						text: $(this).attr('data-text'),
						value: $(this).attr('data-value')
					});
					
					$(this).parent().addClass('highlight').siblings().addClass('highlight');
				}else{
					$(this).parent().removeClass('highlight').siblings().removeClass('highlight');
				}
			});
			//clear selectedItems Table
			o.selectedTable.empty();
			//looping through all items in selectedItems
			$.each(o.selectedItems,function(){
				var val = this.text;
				var itemId = 'itm' + (count++);	// generate item id
				var itm = $('<tr/>');
				var span = $('<label/>').attr('id',itemId)
							.text(this.text);
				itm.append($('<td/>').append(span));
				var label = $('<input/>').attr('type','button').attr('value','X').click(function(){
					
					o.objTable.find('.chk').each(function(){
						if($(this).prop('checked') && $(this).attr('data-text') ==  val){
							$(this).prop('checked',false);
						}
					});	
					self._selChange();
				});
				itm.append($('<td/>').attr('width','25').append(label));
				o.selectedTable.append(itm);
			});
			o.selectedTitle.text(count+' languages selected')
			try{
				// fire onChange event
				o.onChange.call();
			}catch(error){
				console.log('function onChange not found');			
			}
		},

		_filter: function(filter){
			var self = this, o = self.options, el = self.element;

			o.objTable.find('.chk').each(function(){	
				if($(this).attr('data-text').toLowerCase().indexOf(filter.toLowerCase())>-1)
				{
					$(this).parent().parent().show(o.effect);
				}
				else{
					$(this).parent().parent().hide(o.effect);
				}
			});
		},

		getSelection: function(){
			var self = this, o = self.options, el = self.element;
			return o.selectedItems;
		},
		getSelectionStr: function () {
			var self = this, o = self.options, el = self.element;
			let re;
			o.selectedItems.forEach(function (item) {
				re += item.value + ','				
			});
			return re;
		},
		setSelectedData: function(dataModel){
			var self = this, o = self.options, el = self.element;
			$.each(dataModel,function(){
				var item = this;
				o.objTable.find('.chk').each(function(index){
					if($(this).attr('data-text') == item.text){
						$(this).prop('checked',true);
					}
				});
			});
			self._selChange();
		},
		setData: function(dataModel){
			var self = this, o = self.options, el = self.element;
			o.listItems = dataModel;
			self.loadList();
			self._selChange();
		}
	});
	
})(jQuery); 
