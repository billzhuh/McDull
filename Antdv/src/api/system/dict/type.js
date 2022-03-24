import request from '@/utils/request'

// 查询字典类型列表
export function listType (query) {
  return request({
    url: '/system/dict/type/list',
    method: 'get',
    params: query
  })
}

// 查询字典类型详细
export function getType (dictId) {
  return request({
    url: '/system/dict/type/' + dictId,
    method: 'get'
  })
}

// 新增字典类型
export function addType (data) {
  return request({
    url: '/system/dict/type/edit',
    method: 'post',
    data: data
  })
}

// 修改字典类型
export function updateType (data) {
  return request({
    url: '/system/dict/type/edit',
    method: 'put',
    data: data
  })
}

// 删除字典类型
export function delType (dictId) {
  return request({
    url: '/system/dict/type/' + dictId,
    method: 'delete'
  })
}

// 导出
export function exportDict() {
  return request({
    url: '/system/dict/type/export',
    method: 'get',
    //params: query
  })
}


// 刷新参数缓存
export function refreshCache () {
  return request({
    url: '/system/dict/type/refreshCache',
    method: 'delete'
  })
}

// 获取字典选择框列表
export function optionselect () {
  return request({
    //url: '/system/dict/type/optionselect',
    url:'/system/dict/type/list',
    method: 'get'
  })
}
