$(function () {
  var pagedContent = $('.paged-content');
  pagedContent.on('click', '[data-page-button]', function (evt) {
    var url = $(evt.currentTarget).attr('href');
    pagedContent.load(url);
    evt.preventDefault();
    evt.stopPropagation();
  });
});