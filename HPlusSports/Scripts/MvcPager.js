$(function () {
  var pagedContent = $('.paged-content');

  pagedContent.on('click', '[data-page-button]', function (evt) {
    evt.stopPropagation();
    var url = $(evt.currentTarget).attr('href');
    console.debug('Navigating to page ' + url);
    pagedContent.load(url);
    return false;
  });

});