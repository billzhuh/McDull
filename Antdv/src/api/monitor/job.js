import request from '@/utils/request'

// 查询定时任务调度列表
export function listJob (query) {
  return request({
    url: '/system/tasks/list',
    method: 'get',
    params: query
  })
}

// 查询定时任务调度详细
export function getJob (jobId) {
  return request({
    url: '/system/tasks/' + jobId,
    method: 'get'
  })
}

// 新增定时任务调度
export function addJob (data) {
  return request({
    url: '/system/tasks/create',
    method: 'post',
    data: data
  })
}

// 修改定时任务调度
export function updateJob (data) {
  return request({
    url: '/system/tasks/update',
    method: 'post',
    data: data
  })
}

// 删除定时任务调度
export function delJob (jobId) {
  return request({
    url: '/system/tasks/' + jobId,
    method: 'delete'
  })
}

// 任务状态修改
export function changeJobStatus (id,isStart) {
  const data = {
    id,
    isStart
  }
  return request({
    url: '/system/tasks/stop',
    method: 'put',
    data: data
  })
}

// 定时任务立即执行一次
export function runJob(id) {
  return request({
    url: '/system/tasks/run?id=' + id,
    method: 'get'
  })
}


export function exportTasks(query) {
  return request({
    url: '/system/tasks/export',
    method: 'get',
    params: query
  })
}

// export function runJob (jobId, jobGroup) {
//   const data = {
//     jobId,
//     jobGroup
//   }
//   return request({
//     url: '/monitor/job/run',
//     method: 'put',
//     data: data
//   })
//}
