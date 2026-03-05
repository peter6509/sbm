import MessageConfig from './MessageConfig.js'
export default function (proxy, dataConfig, router, onSelect) {
  const store = proxy.$store
  const userInfo = dataConfig.userInfo
  let _userInfo = store.getters.getUserInfo()
  if (_userInfo) {
    userInfo.name = _userInfo.userName
    if (_userInfo.img) {
      userInfo.img = proxy.base.getImgSrc(
        _userInfo.img,
        proxy.$global.oss?.url || proxy.http.ipAddress
      )
    }
  }

  proxy.http.get('api/menu/getTreeMenu', {}, false).then((result) => {
    const navMenuList = dataConfig.navMenuList
    const navCurrentMenuId = dataConfig.navCurrentMenuId
    const menuOptions = dataConfig.menuOptions
    const selectId = dataConfig.selectId
    const menuData = dataConfig.menuData

    let data = result.menu
    let dataItem = data.find((x) => {
      return x.linkType == 3 && (!x.enable || x.enable == 1)
    })
    if (dataItem && dataConfig.layout.value != 'top') {
      dataConfig.links.value.unshift({
        text: '大屏數據',
        icon: 'el-icon-monitor',
        left: true,
        click: () => {
          proxy.http.getDataViewAccessToken({ view: 1 })
        }
      })
    }
    if (dataConfig.layout.value != 'classics') {
      navMenuList.push(
        ...data.filter((c) => {
          return !c.parentId
        })
      )
    }
    data.push({ id: '0', name: '首頁', url: '/home', icon: 'bi-house' }) // 為了獲取選中id使用

    initQueryParams(data)

    store.dispatch('setPermission', data)
    store.commit('setServiceList', result.service)
    store.getters.data().deptList = result.deptList || []
    //字段權限缓存
    store.getters.data().authFields = result.fields || []
    if (navMenuList.length) {
      navMenuList.forEach((m) => {
        m.children = data.filter((c) => {
          return c.parentId == m.id
        })
        m.children.forEach((c) => {
          c.parentId = 0
        })
        for (let index = 0; index < m.children.length; index++) {
          const mItem = m.children[index]
          let mChildrenItems = data.filter((c) => {
            return c.parentId == mItem.id
          })
          m.children.push(...mChildrenItems)
        }
      })
      let navMenuIndex = navMenuList.findIndex((c) => {
        return c.id === dataConfig.navCurrentMenuId.value
      })
      if (navMenuIndex == -1) {
        navCurrentMenuId.value = navMenuList[0].id
        menuData.push(...navMenuList[0].children)
      } else {
        menuData.push(...navMenuList[navMenuIndex].children)
      }
    } else {
      menuData.push(...data)
    }

    menuOptions.value = data

    dataConfig.permissionInited.value = true

    //開啟消息推送（main.js中設置是否開啟signalR)
    if (proxy.$global.signalR) {
      MessageConfig(proxy.http, (result) => {
        // messageList.unshift(result)
        //    console.log(result)
      })
    }

    //當前刷新是不是首頁
    if (router.currentRoute.value.path != dataConfig.navigation[0].path) {
      //查找系统菜單
      let item = menuOptions.value.find((x) => {
        return x.url && x.url == router.currentRoute.value.fullPath
      })
      if (!item) {
        item = menuOptions.value.find((x) => {
          return x.path == router.currentRoute.value.path
        })
      }
      if (item) return onSelect(item.id)
      //查找頂部快捷連接
      item = dataConfig.links.value.find((x) => {
        return x.path == router.currentRoute.value.path
      })
      //查找最后一次跳轉的頁面
      if (!item) {
        item = getItem(proxy, router)
      }
      if (item) {
        return proxy.$tabs.open(item, false)
      } else {
        const indexVal = dataConfig.navigation.findIndex(x => { return x.path == router.currentRoute.value.path })
        if (indexVal != -1) {
          selectId.value = indexVal + '';
          return;
        }
      }
    }
    selectId.value = '0'
  })
}

const getItem = (proxy, router) => {
  let item =
    router.options.routes[0].children.find((x) => {
      return x.path == router.currentRoute.value.path
    }) || {}
  //生成的編輯頁面tabs名稱
  if (item.meta && item.meta.name) {
    let name = item.meta.name
    if (item.meta.edit) {
      name =
        proxy.$ts(name) +
        (router.currentRoute.value.query.id
          ? '(' + proxy.$ts('編輯') + ')'
          : '(' + proxy.$ts('新建') + ')')
    }
    item = {
      name: name,
      path: router.currentRoute.value.path,
      query: router.currentRoute.value.query
    }
    return proxy.$tabs.open(item, false)
  } else {
    let nav = localStorage.getItem(window.location.origin + '_tabs')
    return nav ? JSON.parse(nav) : null
  }
  //  return null;
}

const initQueryParams = (data) => {
  for (let index = 0; index < data.length; index++) {
    const d = data[index]
    d.pid = d.parentId
    if (d.url && d.url.indexOf('?') != -1) {
      let _arr = d.url.split('?')
      d.path = _arr[0]
      _arr = _arr[1].split('&')
      let queryObj = {}
      for (let i = 0; i < _arr.length; i++) {
        // 遍历参數
        if (_arr[i].indexOf('=') != -1) {
          // 如果参數中有值
          let str = _arr[i].split('=')
          queryObj[str[0]] = str[1]
        }
      }
      d.query = queryObj
    } else {
      d.path = d.url
    }
    d.to = d.url
    // if (d.name=='消息記錄') {
    //   var aa=1;
    // }
    // if (!d.icon) {
    //   d.icon = 'el-icon-menu'
    // }
  }
}
