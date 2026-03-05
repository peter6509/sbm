import { nextTick, computed } from 'vue'
export default function (proxy, dataConfig, router) {
  const open = (item, useRoute) => {
    openTabs(proxy, dataConfig, item, useRoute, router)
  }

  const close = (value) => {
    closeTabs(proxy, dataConfig, router, value)
  }
  //導航右鍵關閉
  const navCloseTabs = (value) => {
    onNavCloseTabs(proxy, dataConfig, value)
  }
  const removeNav = (index) => {
    onRemoveNav(proxy, dataConfig, router, index)
  }

  const isDynamicPage = (item) => {
    return checkDynamicPage(router, item)
  }

  const navRefreshPage = (item) => {
    proxy.$tabs.reload(router.currentRoute.value.name)
  }
  const selectNav = (item) => {
    const navigation = dataConfig.navigation
    dataConfig.selectId.value = item.props.name
    let _path = navigation[item.index].path
    dataConfig.currentMenuId.value =
      (
        dataConfig.menuOptions.value.find((c) => {
          return c.path == _path
        }) || { id: 0 }
      ).id * 1

    router.push({
      path: navigation[item.index].path,
      query: navigation[item.index].query
    })
    toDynamicPage(proxy, dataConfig, navigation[item.index], router)
  }

  const breadcrumb = computed(() => {
    const menuItem = dataConfig.menuOptions.value.find((x) => {
      return x.id + '' === dataConfig.currentMenuId.value + ''
    })
     if (!menuItem) {
      return [dataConfig.navigation[dataConfig.navigation.length-1]]
    }
//    console.log(menuItem)
    if (menuItem.id === '0') {
      return [menuItem]
    }
   
    //   menuItem=dataConfig.navigation[dataConfig.navigation.length-1];
    const res = [menuItem]

    for (let index = 0; index < res.length; index++) {
      const item = res[index]
      const par = dataConfig.menuOptions.value.find((x) => {
        return x.id && x.id == item.pid&&item.pid
      })
      const b =
        par &&
        !res.some((x) => {
          return x.id == par.id
        })
      if (b) {
        res.push(par)
      }
    }
    return res.reverse()
  })

  return {
    navCloseTabs,
    open,
    close,
    selectNav,
    removeNav,
    isDynamicPage,
    navRefreshPage,
    breadcrumb
  }
}

const toDynamicPage = (proxy, dataConfig, item, router) => {
  if (!item) {
    console.log('item is null')
    return
  }
  const navigation = dataConfig.navigation
  if (checkDynamicPage(router, item)) {
    //navigation[item.index]
    const _path = item.path
    router.push({ path: _path, query: item.query }) // navigation[item.index].query
    nextTick(() => {
      setTimeout(() => {
        proxy.$tabs.reload &&
          proxy.$tabs.reload(getRouteOption(router, _path).name || router.currentRoute.value.name)
      }, 50)
    })
  }
}

//導航右鍵關閉
const onNavCloseTabs = (proxy, dataConfig, value) => {
  const navigation = dataConfig.navigation
  const selectId = dataConfig.selectId
  const selectMenuIndex = dataConfig.selectMenuIndex
  //首頁右鍵
  if (selectId.value == '-1') {
    selectId.value = 0
    value = ''
  }
  let _menuId = navigation[selectId.value * 1].id
  let currnetIndex = selectId.value * 1 // navigation.findIndex(c => { return c.id == selectId.value });
  switch (value) {
    case 'left': {
      // 删除左側tab標簽
      navigation.splice(1, currnetIndex - 1) // 删除左側tab標簽
      break
    }
    case 'right': {
      // 删除右側tab標簽
      if (selectMenuIndex.value == 0) {
        navigation.splice(currnetIndex) // 删除右側tab標簽
        toHome(proxy, navigation)
      } else {
        navigation.splice(currnetIndex + 1) // 删除右側tab標簽
        if (selectMenuIndex.value < currnetIndex) {
          navigation.splice(selectMenuIndex.value, currnetIndex - selectMenuIndex.value)
        }
      }
      break
    }
    case 'other': {
      // 删除其他所有tab標簽
      navigation.splice(currnetIndex + 1) // 删除右側tab標簽(這里必須按照右→左顺序删除)
      navigation.splice(1, currnetIndex - 1) // 删除左側tab標簽
      break
    }
    default: {
      //關閉所有
      navigation.splice(1, navigation.length)
      toHome(proxy, navigation)
      break
    }
  }
  selectId.value =
    navigation.findIndex((c) => {
      return c.id == _menuId
    }) + ''

  dataConfig.contextMenuVisible.value = false
}

const toHome = (proxy, navigation) => {
  proxy.$tabs.open({
    text: navigation[0].name,
    path: navigation[0].path
  })
}
//動態頁面
const checkDynamicPage = (router, item) => {
  const b = router.getRoutes().some((x) => {
    return x.path == item.path && x.meta && x.meta.dynamic
  })
  return b
}

const openTabs = (proxy, dataConfig, item, useRoute, router) => {
  const navigation = dataConfig.navigation
  const selectId = dataConfig.selectId
  const currentMenuId = dataConfig.currentMenuId

  const dynamicPage = checkDynamicPage(router, item)
  let _index = navigation.findIndex((x) => {
    if (dynamicPage) {
      return x.path == item.path && x.name == item.name
    }
    return x.path == item.path
  })
  let b = false
  if (navigation[selectId * 1] && navigation[selectId * 1].path == item.path) {
    b = true
    //同一個頁面不再跳轉
    if (navigation[_index].name == item.name) {
      if (dynamicPage) {
        proxy.$tabs.reload && proxy.$tabs?.reload(router.currentRoute.value.name)
      }
      return
    }
  }
  //如果打開的是表單編輯設置tabs的標題
  setTabsName(proxy, dataConfig, item, _index)
  if (_index == -1) {
    navigation.push({
      //  orderNo: String(navigation.length),// 序號
      id: item.id + '',
      name: item.name || item.text || '無標題',
      path: item.path,
      query: item.query //2021.03.20修複自定義二次打開$tabs時参數丢失的問題
    })
    //新打開的tab移至最后一個選項
    selectId.value = navigation.length - 1 + ''
  } else {
    navigation[_index].query = item.query
    selectId.value = _index + ''
  }
  if (useRoute === undefined) {
    //非標準菜單，記錄最后一次跳轉的頁面，用于刷新
    setItem(item)
    router.push(item)
    // this.$router.push(item);
  }
  if (dynamicPage) {
    toDynamicPage(proxy, dataConfig, item, router)
    // router.push({ path: item.path, query: item.query })
    // nextTick(() => {
    //     // proxy.$tabs.reload && proxy.$tabs.reload(getRouteOption(router, item.path).name)
    //     setTimeout(() => {
    //         proxy.$tabs.reload && proxy.$tabs.reload(getRouteOption(router, item.path).name || router.currentRoute.value.name)
    //         // proxy.$tabs.reload && proxy.$tabs.reload(router.currentRoute.value.name);
    //     }, 50);
    // });
    // setTimeout(() => {
    //     proxy.$tabs.reload()
    //     //refreshPage()
    // }, 50)
    // return;
  }
  currentMenuId.value = item.id * 1
  // tab菜單绑定右鍵事件
  nextTick(function (e) {
    bindRightClickMenu(true, proxy, dataConfig, router)
  })
}
const closeTabs = (proxy, dataConfig, router, path) => {
  /* 2020.07.31增加手動打開tabs*/
  let index = dataConfig.navigation.findIndex((x) => {
    return x.path == path
  })
  if (index == -1) {
    return proxy.$Message.error('未找到菜單')
  }
  onRemoveNav(proxy, dataConfig, router, index)
}

export const onRemoveNav = (proxy, dataConfig, router, index) => {
  proxy.$tabs.clearCache()
  return new Promise(() => {
    const navigation = dataConfig.navigation
    const selectId = dataConfig.selectId
    const currentMenuId = dataConfig.currentMenuId
    //關閉的當前項,跳轉到前一個頁面
    if (selectId.value == index + '') {
      //console.log(navigation[_index - 1])
      setItem(navigation[index - 1])
      router.push({
        path: navigation[index - 1].path,
        //2022.06.27修複tabs二次切换后参數丢失的問題
        query: navigation[index - 1].query
      })
      navigation.splice(index, 1)
      selectId.value = selectId.value - 1 + ''
      toDynamicPage(proxy, dataConfig, dataConfig.navigation[index - 1], router)
      return
    }
    if (index < selectId.value) {
      selectId.value = selectId.value - 1 + ''
    }
    navigation.splice(index, 1)
    currentMenuId.value =
      (
        dataConfig.menuOptions.value.find((c) => {
          return c.path == navigation[selectId.value * 1].path
        }) || { id: 0 }
      ).id * 1
  })
}

const setItem = (item) => {
  /* 2020.07.31增加手動打開tabs*/
  localStorage.setItem(window.location.origin + '_tabs', JSON.stringify(item))
}

const getRouteOption = (router, path) => {
  return router.options.routes[0].children.find((x) => {
    return x.path == path
  })
}
const setTabsName = (proxy, dataConfig, item, _index) => {
  const navigation = dataConfig.navigation
  const router = proxy.$router
  const routeOption = getRouteOption(router, item.path)
  if (!routeOption) {
    return
  }
  if (routeOption.meta && routeOption.meta.name && routeOption.meta.edit) {
    if (!item.query) {
      item.query = {}
    }
    let _item = navigation.find((c) => {
      return c.path == item.path
    })
    if (_item) {
      _item.query = item.query
    }
    let id = _index == -1 ? router.currentRoute.value.query.id : item.query.id
    if (!id) {
      id = item.query.id
    }
    item.text = routeOption.meta.name //+ (id ? '(' + $ts('編輯') + ')' : '(' + $ts('新建') + ')');
  }
  if (_index != -1) {
    navigation[_index].name = item.text || item.name
  }
}

const bindRightClickMenu = (enable, proxy, dataConfig, router) => {
  if (!enable) return
  // 使用原生js 為單個dom绑定鼠標右擊事件
  nextTick(() => {
    let tab_top_dom = Object.assign([], document.getElementsByClassName('el-tabs__item is-top'))
    tab_top_dom.forEach((item, index) => {
      item.oncontextmenu = (e) => {
        openTabsMenu(e, proxy, dataConfig, router)
      }
    })
  })
}

/**
 * 顯示右鍵菜單
 * @param {*} e 事件對象
 */
const openTabsMenu = function (e, proxy, dataConfig) {
  const navigation = dataConfig.navigation
  e.preventDefault() // 防止默認菜單彈出
  let tabId = e.target.id.split('-')[1] * 1

  //記錄當前選中的菜單index
  dataConfig.selectMenuIndex.value =
    document.getElementById('pane-' + tabId).children[0].textContent * 1
  //只有首頁時不顯示
  if (navigation.length == 1) {
    return
  }

  const visibleItem = dataConfig.visibleItem
  //首頁設置顯示關閉右邊菜單
  if (!dataConfig.selectMenuIndex.value) {
    visibleItem.all = false
    visibleItem.right = true
    visibleItem.left = false
    visibleItem.other = false
  } else {
    visibleItem.all = true
    //不是最后一個顯示關閉右邊菜單
    visibleItem.right = dataConfig.selectMenuIndex.value != navigation.length - 1
    //只有兩個菜單時不顯示關閉左邊
    visibleItem.left = navigation.length != 2
    //只有兩個菜單時不顯示關閉其他
    visibleItem.other = navigation.length != 2
  }
  dataConfig.contextMenuVisible.value = true
  // 設置右鍵菜單顯示的位置
  dataConfig.menuLeft.value = e.target.getBoundingClientRect().left + 1 //- (isCollapse.value ? 63 : 198); //-e.target.clientWidth
  dataConfig.menuTop.value = e.target.getBoundingClientRect().top + e.target.clientHeight + 4
}
