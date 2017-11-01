var today = new Date();
var tomorrow = new Date(new Date().getTime() + 24 * 60 * 60 * 1000);
var dd = today.getDate();
var mm = today.getMonth() + 1; //January is 0!
var yyyy = today.getFullYear();
var tomday = tomorrow.getDate();
var tommonth = tomorrow.getMonth() + 1;
var tomyear = tomorrow.getFullYear();
if (dd < 10) { dd = '0' + dd } if (mm < 10) { mm = '0' + mm } today = mm + '/' + dd + '/' + yyyy;
if (tomday < 10) { tomday = '0' + tomday } if (tommonth < 10) { tommonth = '0' + tommonth } tomorrow = tommonth + '/' + tomday + '/' + tomyear;
$('#theDate').attr('value', today);
$('#tomorrowDate').attr('value', tomorrow);