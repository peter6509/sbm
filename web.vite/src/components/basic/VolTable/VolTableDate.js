export const compareDate = (date1, date2) => {
  if (!date2) {
    return true
  }
  return date1.valueOf() < (typeof date2 == 'number' ? date2 : new Date(date2).valueOf())
}

export const getDateOptions = (date, item) => {
  //2021.07.17設置時間可選範圍
  if ((!item.min && !item.max) || !date) {
    return false
  }
  if (item.min && item.min.indexOf(' ') == -1) {
    //不設置時分秒，后面會自動加上 08:00
    item.min = item.min + ' 00:00:000'
  }
  return compareDate(date, item.min) || !compareDate(date, item.max)
}

export const  getDateFormat=(column)=> {
    if (column.format) {
      return column.format;
    }
    if (column.edit.type == "month") {
      return "YYYY-MM";
    }
    //見https://day.js.org/docs/zh-CN/display/format
    return column.edit.type == "date" ? "YYYY-MM-DD" : "YYYY-MM-DD HH:mm:ss";
  }