(function () {
    var panel = document.getElementById('panel');
    var fixed = document.querySelector('.fixed-header');
    var leftMenu = document.getElementById('left-menu');
    var rightMenu = document.getElementById('right-menu');

  var leftSlideout = new  Slideout({
      'panel': panel,
    'menu': leftMenu,
    'padding': 256,
    'tolerance': 70,
    'touch': false
  });
  var rightSlideout = new  Slideout({
      'panel': panel,
    'menu': rightMenu,
    'padding': 256,
    'tolerance': 70,
    'side': 'right',
    'touch': false
  });

  document
    .querySelector('#toggle-left')
    .addEventListener('click', function() {
      leftSlideout.toggle();
      leftMenu.style.zIndex = '0';
    });

  leftSlideout.on('close', function () {
      leftMenu.style.zIndex = '-1';
      rightMenu.style.zIndex = '-1';
      fixed.style.transition = '';
  });

  leftSlideout.on('open', function () {
      leftMenu.style.zIndex = '0';
      rightMenu.style.zIndex = '-1';
      fixed.style.transition = '';
  });

  leftSlideout.on('translate', function (translated) {
      leftMenu.style.zIndex = '0';
      rightMenu.style.zIndex = '-1';
      fixed.style.transform = 'translateX(' + translated + 'px)';
  });

  document
    .querySelector('#toggle-right')
    .addEventListener('click', function() {
      rightSlideout.toggle();
      rightMenu.style.zIndex = '0';
      Djinaro.setDisplayElement('right-menu-main', 'block');
      Djinaro.setDisplayElement('brands', 'none');
      Djinaro.setDisplayElement('sizes', 'none');
      Djinaro.setDisplayElement('pricerange', 'none');
    });

  rightSlideout.on('close', function () {
      rightMenu.style.zIndex = '-1';
      leftMenu.style.zIndex = '-1';
      fixed.style.transition = '';
  });

  rightSlideout.on('open', function () {
      rightMenu.style.zIndex = '0';
      leftMenu.style.zIndex = '-1';
      fixed.style.transition = '';
  });

  rightSlideout.on('translate', function (translated) {
      rightMenu.style.zIndex = '0';
      leftMenu.style.zIndex = '-1';
      fixed.style.transform = 'translateX(' + translated + 'px)';
  });

    

})();
