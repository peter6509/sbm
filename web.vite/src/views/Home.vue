<template>
  <div class="home-container">
    <el-scrollbar style="height: 100%">
      <!-- 頂部統計卡片 -->
      <div class="stat-cards">
        <div
          class="stat-card"
          v-for="(item, index) in statCards"
          :key="index"
          :style="{ background: item.gradient }"
        >
          <div class="card-icon">
            <i :class="item.icon"></i>
          </div>
          <div class="card-content">
            <div class="card-value">
              {{ formatNumber(item.value) }}
            </div>
            <div class="card-label">{{ item.label }}</div>
          </div>
        </div>
      </div>

      <!-- 圖表區域 -->
      <div class="chart-section">
        <div class="chart-box chart-left">
          <div class="chart-title">任務狀態分佈</div>
          <div id="taskStatusChart" class="chart-container"></div>
        </div>
        <div class="chart-box chart-right">
          <div class="chart-title">
            <span>近期任務完成趨勢</span>
            <el-radio-group v-model="trendDays" size="small" @change="loadTrendChart">
              <el-radio-button :value="7">7天</el-radio-button>
              <el-radio-button :value="14">14天</el-radio-button>
              <el-radio-button :value="30">30天</el-radio-button>
            </el-radio-group>
          </div>
          <div id="trendChart" class="chart-container"></div>
        </div>
      </div>

      <!-- 專案進度與待辦事項 -->
      <div class="bottom-section">
        <div class="project-progress-box">
          <div class="section-title">
            <span>專案進度概覽</span>
            <el-button type="primary" link @click="goToProjects">查看全部</el-button>
          </div>
          <div class="project-list">
            <div class="project-item"
  v-for="project in projectProgress"
  :key="project.project_id"
  @click="goToProjectDetail(project.project_id)"
>
  <div class="project-info">
    <div class="project-name">{{ project.name }}</div>
    <div class="project-meta">
      <span>{{ project.project_no }}</span>
      <span class="divider">|</span>
      <span>任務: {{ project.completed_tasks }}/{{ project.total_tasks }}</span>
      <!-- 專案負責人 -->
      <span class="divider" v-if="project.user_name">|</span>
      <span class="user-info" v-if="project.user_name">
        <el-icon><User /></el-icon>
        {{ project.user_name }}
      </span>
    </div>
  </div>
  <div class="project-progress">
    <el-progress
      :percentage="project.progress"
      :stroke-width="10"
      :color="getProgressColor(project.progress)"
    />
  </div>
</div>
            <el-empty v-if="projectProgress.length === 0" description="暫無專案資料" />
          </div>
        </div>

        <div class="todo-box">
          <div class="section-title">
            <span>即將到期任務</span>
            <el-tag type="danger" size="small">{{ upcomingTasks.length }} 項</el-tag>
          </div>
          <div class="todo-list">
            <div class="todo-item"
  v-for="task in upcomingTasks"
  :key="task.sub_id"
  @click="goToTask(task)"
>
  <div class="todo-status">
    <el-tag :type="getStatusType(task.status)" size="small">
      {{ getStatusText(task.status) }}
    </el-tag>
  </div>
  <div class="todo-content">
    <div class="todo-name">{{ task.sub_name }}</div>
    <div class="todo-project">
      {{ task.project_name }}
      <!-- 任務負責人 -->
      <span class="user-info" v-if="task.user_name">
        <el-icon><User /></el-icon>
        {{ task.user_name }}
      </span>
    </div>
  </div>
  <div class="todo-date" :class="{ overdue: isOverdue(task.end_date) }">
    {{ formatDate(task.end_date) }}
    <span v-if="isOverdue(task.end_date)" class="overdue-tag">已逾期</span>
    <span v-else-if="isDueSoon(task.end_date)" class="due-soon-tag">即將到期</span>
  </div>
</div>
            <el-empty v-if="upcomingTasks.length === 0" description="暫無待辦任務" />
          </div>
        </div>
      </div>

      <!-- 逾期任務警告區 -->
      <div class="overdue-section" v-if="overdueTasks.length > 0">
        <div class="section-title warning">
          <el-icon><WarningFilled /></el-icon>
          <span>逾期任務警告</span>
          <el-tag type="danger" size="small">{{ overdueTasks.length }} 項</el-tag>
        </div>
        <div class="overdue-list">
          <div class="overdue-item"
  v-for="task in overdueTasks"
  :key="task.sub_id"
  @click="goToTask(task)"
>
  <div class="overdue-info">
    <span class="task-name">{{ task.sub_name }}</span>
    <span class="project-name">
      {{ task.project_name }}
      <!-- 任務負責人 -->
      <span class="user-info" v-if="task.user_name">
        <el-icon><User /></el-icon>
        {{ task.user_name }}
      </span>
    </span>
  </div>
  <div class="overdue-days">
    逾期 {{ getOverdueDays(task.end_date) }} 天
  </div>
</div>
        </div>
      </div>
    </el-scrollbar>
  </div>
</template>

<script setup>
import { ref, onMounted, onUnmounted, getCurrentInstance } from 'vue'
import { WarningFilled, User } from '@element-plus/icons-vue'
import * as echarts from 'echarts'

const { proxy } = getCurrentInstance()

// API 基礎路徑
const API_BASE = '/api/sf_project_dashboard'

// 圖表實例
let taskStatusChart = null
let trendChart = null

// 統計卡片數據
const statCards = ref([
  {
    label: '新業務案量',
    value: 0,
    icon: 'el-icon-folder-opened',
    gradient: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)'
  },
  {
    label: '進行中專案',
    value: 0,
    icon: 'el-icon-loading',
    gradient: 'linear-gradient(135deg, #11998e 0%, #38ef7d 100%)'
  },
  {
    label: '已結案專案',
    value: 0,
    icon: 'el-icon-document',
    gradient: 'linear-gradient(135deg, #2196F3 0%, #21CBF3 100%)'
  },
  // {
  //   label: '待處理任務',
  //   value: 0,
  //   icon: 'el-icon-time',
  //   gradient: 'linear-gradient(135deg, #f093fb 0%, #f5576c 100%)'
  // },
  // {
  //   label: '進行中任務',
  //   value: 0,
  //   icon: 'el-icon-video-play',
  //   gradient: 'linear-gradient(135deg, #4facfe 0%, #00f2fe 100%)'
  // },
  // {
  //   label: '逾期任務',
  //   value: 0,
  //   icon: 'el-icon-warning',
  //   gradient: 'linear-gradient(135deg, #eb3349 0%, #f45c43 100%)'
  // }
])

// 趨勢圖天數
const trendDays = ref(7)

// 專案進度列表
const projectProgress = ref([])

// 即將到期任務
const upcomingTasks = ref([])

// 逾期任務
const overdueTasks = ref([])

// 任務狀態配置
const statusConfig = {
  0: { text: '計劃評估', type: 'info', color: '#909399' },
  1: { text: '準備中', type: 'warning', color: '#E6A23C' },
  2: { text: '進行中', type: 'primary', color: '#409EFF' },
  3: { text: '審查中', type: 'success', color: '#67C23A' },
  4: { text: '已完成', type: 'success', color: '#52c41a' }
}

// 格式化數字
const formatNumber = (num) => {
  return (num + '').replace(/\B(?=(\d{3})+(?!\d))/g, ',')
}

// 格式化日期
const formatDate = (date) => {
  if (!date) return '-'
  return new Date(date).toLocaleDateString('zh-TW')
}

// 判斷是否逾期
const isOverdue = (date) => {
  if (!date) return false
  return new Date(date) < new Date(new Date().toDateString())
}

// 判斷是否即將到期（3天內）
const isDueSoon = (date) => {
  if (!date) return false
  const endDate = new Date(date)
  const today = new Date(new Date().toDateString())
  const diffDays = Math.ceil((endDate - today) / (1000 * 60 * 60 * 24))
  return diffDays >= 0 && diffDays <= 3
}

// 獲取逾期天數
const getOverdueDays = (date) => {
  if (!date) return 0
  const endDate = new Date(date)
  const today = new Date(new Date().toDateString())
  return Math.ceil((today - endDate) / (1000 * 60 * 60 * 24))
}

// 獲取狀態文字
const getStatusText = (status) => {
  return statusConfig[status]?.text || '未知'
}

// 獲取狀態標籤類型
const getStatusType = (status) => {
  return statusConfig[status]?.type || 'info'
}

// 獲取進度條顏色
const getProgressColor = (percentage) => {
  if (percentage < 30) return '#f56c6c'
  if (percentage < 70) return '#e6a23c'
  return '#67c23a'
}

// 載入統計數據
const loadStatistics = async () => {
  // try {
  //   const res = await proxy.http.post(`${API_BASE}/GetStatistics`, {})
  //   if (res.status) {
  //     const data = res.data
  //     statCards.value[0].value = data.totalProjects
  //     statCards.value[1].value = data.activeProjects
  //     statCards.value[2].value = data.totalTasks
  //     statCards.value[3].value = data.pendingTasks
  //     statCards.value[4].value = data.inProgressTasks
  //     statCards.value[5].value = data.overdueTasks
  //   }
  // } catch (error) {
  //   console.error('載入統計數據失敗:', error)
  // }
}

// 載入任務狀態分佈圖表
const loadTaskStatusChart = async () => {
  // try {
  //   const res = await proxy.http.post(`${API_BASE}/GetTaskStatusDistribution`, {})
  //   if (res.status && taskStatusChart) {
  //     const data = res.data
  //     taskStatusChart.setOption({
  //       tooltip: {
  //         trigger: 'item',
  //         formatter: '{b}: {c} ({d}%)'
  //       },
  //       legend: {
  //         orient: 'vertical',
  //         right: 10,
  //         top: 'center',
  //         itemWidth: 12,
  //         itemHeight: 12
  //       },
  //       color: ['#909399', '#E6A23C', '#409EFF', '#67C23A', '#52c41a'],
  //       series: [
  //         {
  //           type: 'pie',
  //           radius: ['45%', '70%'],
  //           center: ['40%', '50%'],
  //           avoidLabelOverlap: false,
  //           itemStyle: {
  //             borderRadius: 6,
  //             borderColor: '#fff',
  //             borderWidth: 2
  //           },
  //           label: {
  //             show: false
  //           },
  //           emphasis: {
  //             label: {
  //               show: true,
  //               fontSize: 14,
  //               fontWeight: 'bold'
  //             }
  //           },
  //           data: [
  //             { value: data.planning, name: '計劃評估' },
  //             { value: data.preparing, name: '準備中' },
  //             { value: data.inProgress, name: '進行中' },
  //             { value: data.reviewing, name: '審查中' },
  //             { value: data.completed, name: '已完成' }
  //           ]
  //         }
  //       ]
  //     })
  //   }
  // } catch (error) {
  //   console.error('載入任務狀態分佈失敗:', error)
  // }
}

// 載入趨勢圖表
const loadTrendChart = async () => {
  // try {
  //   const res = await proxy.http.post(`${API_BASE}/GetTaskTrend`, {
  //     days: trendDays.value
  //   })
  //   if (res.status && trendChart) {
  //     const data = res.data
  //     trendChart.setOption({
  //       tooltip: {
  //         trigger: 'axis',
  //         axisPointer: {
  //           type: 'shadow'
  //         }
  //       },
  //       legend: {
  //         data: ['新增任務', '完成任務'],
  //         top: 0
  //       },
  //       grid: {
  //         left: 50,
  //         right: 20,
  //         bottom: 30,
  //         top: 40
  //       },
  //       xAxis: {
  //         type: 'category',
  //         data: data.dates,
  //         axisLabel: {
  //           rotate: 30
  //         }
  //       },
  //       yAxis: {
  //         type: 'value',
  //         splitLine: {
  //           lineStyle: {
  //             type: 'dashed'
  //           }
  //         }
  //       },
  //       series: [
  //         {
  //           name: '新增任務',
  //           type: 'bar',
  //           barWidth: 12,
  //           itemStyle: {
  //             color: new echarts.graphic.LinearGradient(0, 0, 0, 1, [
  //               { offset: 0, color: '#83bff6' },
  //               { offset: 1, color: '#188df0' }
  //             ]),
  //             borderRadius: [4, 4, 0, 0]
  //           },
  //           data: data.created
  //         },
  //         {
  //           name: '完成任務',
  //           type: 'bar',
  //           barWidth: 12,
  //           itemStyle: {
  //             color: new echarts.graphic.LinearGradient(0, 0, 0, 1, [
  //               { offset: 0, color: '#25d053' },
  //               { offset: 1, color: '#17b978' }
  //             ]),
  //             borderRadius: [4, 4, 0, 0]
  //           },
  //           data: data.completed
  //         }
  //       ]
  //     })
  //   }
  // } catch (error) {
  //   console.error('載入趨勢圖表失敗:', error)
  // }
}

// 載入專案進度
const loadProjectProgress = async () => {
  // try {
  //   const res = await proxy.http.post(`${API_BASE}/GetProjectProgress`, {
  //     top: 5
  //   })
  //   if (res.status) {
  //     projectProgress.value = res.data
  //   }
  // } catch (error) {
  //   console.error('載入專案進度失敗:', error)
  // }
}

// 載入即將到期任務
const loadUpcomingTasks = async () => {
  // try {
  //   const res = await proxy.http.post(`${API_BASE}/GetUpcomingTasks`, {
  //     days: 7
  //   })
  //   if (res.status) {
  //     upcomingTasks.value = res.data
  //   }
  // } catch (error) {
  //   console.error('載入即將到期任務失敗:', error)
  // }
}

// 載入逾期任務
const loadOverdueTasks = async () => {
  // try {
  //   const res = await proxy.http.post(`${API_BASE}/GetOverdueTasks`, {})
  //   if (res.status) {
  //     overdueTasks.value = res.data
  //   }
  // } catch (error) {
  //   console.error('載入逾期任務失敗:', error)
  // }
}

// 跳轉到專案列表 (請根據實際路由調整)
const goToProjects = () => {
  proxy.$router.push('/sf_project_instance')
}

// 跳轉到專案詳情
const goToProjectDetail = (projectId) => {
  proxy.$router.push(`/sf_project_instance?project_id=${projectId}`)
}

// 跳轉到任務
const goToTask = (task) => {
  proxy.$router.push(`/sf_project_instance_sub?project_id=${task.project_id}&sub_id=${task.sub_id}`)
}

// 初始化圖表
const initCharts = () => {
  const taskStatusEl = document.getElementById('taskStatusChart')
  const trendEl = document.getElementById('trendChart')
  
  if (taskStatusEl) {
    taskStatusChart = echarts.init(taskStatusEl)
  }
  if (trendEl) {
    trendChart = echarts.init(trendEl)
  }

  // 監聽視窗變化
  window.addEventListener('resize', handleResize)
}

// 處理視窗變化
const handleResize = () => {
  taskStatusChart?.resize()
  trendChart?.resize()
}

// 載入所有數據
const loadAllData = () => {
  loadStatistics()
  loadTaskStatusChart()
  loadTrendChart()
  loadProjectProgress()
  loadUpcomingTasks()
  loadOverdueTasks()
}

onMounted(() => {
  initCharts()
  loadAllData()
})

onUnmounted(() => {
  window.removeEventListener('resize', handleResize)
  taskStatusChart?.dispose()
  trendChart?.dispose()
})
</script>

<style lang="less" scoped>
.home-container {
  position: absolute;
  height: 100%;
  width: 100%;
  background: #f0f2f5;
  padding: 20px;
  box-sizing: border-box;
}

// 統計卡片
.stat-cards {
  display: grid;
  grid-template-columns: repeat(6, 1fr);
  gap: 16px;
  margin-bottom: 20px;
}

.stat-card {
  padding: 20px;
  border-radius: 12px;
  color: #fff;
  display: flex;
  align-items: center;
  gap: 15px;
  cursor: pointer;
  transition: all 0.3s ease;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);

  &:hover {
    transform: translateY(-4px);
    box-shadow: 0 8px 24px rgba(0, 0, 0, 0.15);
  }

  .card-icon {
    width: 50px;
    height: 50px;
    background: rgba(255, 255, 255, 0.2);
    border-radius: 12px;
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 24px;
  }

  .card-content {
    flex: 1;

    .card-value {
      font-size: 28px;
      font-weight: bold;
      line-height: 1.2;
    }

    .card-label {
      font-size: 14px;
      opacity: 0.9;
      margin-top: 4px;
    }
  }
}

// 圖表區域
.chart-section {
  display: flex;
  gap: 20px;
  margin-bottom: 20px;
}

.chart-box {
  background: #fff;
  border-radius: 12px;
  padding: 20px;
  box-shadow: 0 2px 12px rgba(0, 0, 0, 0.05);

  .chart-title {
    font-size: 16px;
    font-weight: 600;
    color: #303133;
    margin-bottom: 15px;
    display: flex;
    justify-content: space-between;
    align-items: center;
  }

  .chart-container {
    height: 280px;
  }
}

.chart-left {
  flex: 1;
}

.chart-right {
  flex: 1.5;
}

// 底部區域
.bottom-section {
  display: flex;
  gap: 20px;
  margin-bottom: 20px;
}

.project-progress-box,
.todo-box {
  flex: 1;
  background: #fff;
  border-radius: 12px;
  padding: 20px;
  box-shadow: 0 2px 12px rgba(0, 0, 0, 0.05);
}

.section-title {
  font-size: 16px;
  font-weight: 600;
  color: #303133;
  margin-bottom: 15px;
  display: flex;
  justify-content: space-between;
  align-items: center;

  &.warning {
    color: #f56c6c;

    .el-icon {
      margin-right: 8px;
    }
  }
}

// 專案列表
.project-list {
  max-height: 320px;
  overflow-y: auto;
}

.project-item {
  padding: 12px;
  border-radius: 8px;
  margin-bottom: 10px;
  background: #fafafa;
  cursor: pointer;
  transition: all 0.2s;

  &:hover {
    background: #f0f7ff;
  }

  &:last-child {
    margin-bottom: 0;
  }

  .project-info {
    margin-bottom: 8px;

    .project-name {
      font-size: 14px;
      font-weight: 500;
      color: #303133;
      margin-bottom: 4px;
    }

    .project-meta {
      font-size: 12px;
      color: #909399;

      .divider {
        margin: 0 8px;
      }
    }
  }
}

// 待辦列表
.todo-list {
  max-height: 320px;
  overflow-y: auto;
}

.todo-item {
  display: flex;
  align-items: center;
  padding: 12px;
  border-radius: 8px;
  margin-bottom: 10px;
  background: #fafafa;
  cursor: pointer;
  transition: all 0.2s;

  &:hover {
    background: #f0f7ff;
  }

  &:last-child {
    margin-bottom: 0;
  }

  .todo-status {
    margin-right: 12px;
  }

  .todo-content {
    flex: 1;

    .todo-name {
      font-size: 14px;
      font-weight: 500;
      color: #303133;
      margin-bottom: 2px;
    }

    .todo-project {
      font-size: 12px;
      color: #909399;
    }
  }
  .user-info {
  display: inline-flex;
  align-items: center;
  gap: 3px;
  color: #606266;
  margin-left: 8px;
  
  .el-icon {
    font-size: 12px;
    color: #909399;
  }
}
.todo-project .user-info {
  padding-left: 8px;
  border-left: 1px solid #e4e7ed;
}
.user-badge {
  margin-left: 8px;
  background: #ecf5ff;
  color: #409eff;
  padding: 2px 8px;
  border-radius: 4px;
  font-size: 11px;
}
  .todo-date {
    font-size: 12px;
    color: #909399;
    display: flex;
    align-items: center;
    gap: 4px;

    &.overdue {
      color: #f56c6c;
    }

    .overdue-tag {
      background: #fef0f0;
      color: #f56c6c;
      padding: 2px 6px;
      border-radius: 4px;
      font-size: 10px;
    }

    .due-soon-tag {
      background: #fdf6ec;
      color: #e6a23c;
      padding: 2px 6px;
      border-radius: 4px;
      font-size: 10px;
    }
  }
}

// 逾期任務區域
.overdue-section {
  background: #fff;
  border-radius: 12px;
  padding: 20px;
  box-shadow: 0 2px 12px rgba(0, 0, 0, 0.05);
  border-left: 4px solid #f56c6c;
  margin-bottom: 20px;
}

.overdue-list {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 12px;
}

.overdue-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 12px;
  background: #fef0f0;
  border-radius: 8px;
  cursor: pointer;
  transition: all 0.2s;

  &:hover {
    background: #fde2e2;
  }

  .overdue-info {
    .task-name {
      font-size: 14px;
      font-weight: 500;
      color: #303133;
      display: block;
      margin-bottom: 2px;
    }

    .project-name {
      font-size: 12px;
      color: #909399;
    }
  }
.overdue-info .user-info {
  color: #909399;
  font-size: 12px;
}
  .overdue-days {
    font-size: 12px;
    font-weight: 600;
    color: #f56c6c;
    background: #fff;
    padding: 4px 10px;
    border-radius: 4px;
  }
}

// 響應式調整
@media screen and (max-width: 1400px) {
  .stat-cards {
    grid-template-columns: repeat(3, 1fr);
  }
}

@media screen and (max-width: 1200px) {
  .chart-section {
    flex-direction: column;
  }

  .bottom-section {
    flex-direction: column;
  }

  .overdue-list {
    grid-template-columns: 1fr;
  }
}

@media screen and (max-width: 768px) {
  .stat-cards {
    grid-template-columns: repeat(2, 1fr);
  }
}
</style>