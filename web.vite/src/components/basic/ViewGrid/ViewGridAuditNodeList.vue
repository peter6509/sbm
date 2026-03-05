<template>
    <div class="v-steps">
        <div v-for="(item, index) in workFlowSteps" :key="index">
            <div class="step-item" v-if="item.stepAttrType == 'start'">
                <div class="left-item">
                    <div>{{ $ts('流程開始') }}<span class="wl-icon el-icon-time"></span> </div>
                    <div class="left-date">{{ item.createDate }}</div>
                </div>
                <div class="right-item">
                    <div class="step-line"></div>
                    <i class="step-circle"></i>
                    <div class="step-title">
                        {{ item.stepName }}
                    </div>
                    <div class="step-text">{{ $ts('發起人') }}：{{ item.creator }}</div>
                </div>
            </div>
            <div class="step-item" v-else-if="item.stepAttrType == 'end'">
                <div class="left-item">
                    <div>{{ $ts('流程结束') }}<span class="wl-icon el-icon-time"></span></div>
                </div>
                <div class="right-item">
                    <div class="step-line"></div>
                    <i class="step-circle"></i>
                    <div class="step-title">
                        {{ item.stepName }}
                    </div>
                </div>
            </div>
            <div v-else :class="{
                'step-current': item.isCurrent,
                'step-item-gray': !item.auditDate
            }" class="step-item">
                <div class="left-item">
                    <div v-if="item.stepAttrType == 'cc'"> {{ $ts('抄送') }}<span class="wl-icon el-icon-s-promotion"></span>
                    </div>
                    <div v-else>{{ $ts('審批時間') }}<span class="wl-icon el-icon-document-checked"></span></div>
                    <div class="left-date">
                        {{ item.stepAttrType == 'cc' ? '' : (item.auditDate || $ts('待審批')) }}
                    </div>
                </div>
                <div class="right-item">
                    <div class="step-line"></div>
                    <i class="step-circle"></i>
                    <div class="step-title">
                        {{ item.stepName }}
                    </div>
                    <template v-if="item.auditList && item.auditList.length">
                        <!-- <div class="step-text">{{ $ts("審批信息") }}</div> -->
                        <div class="step-text">
                            <div v-if="item.stepAttrType == 'cc'">
                                {{ $ts('抄送對象') }}： {{item.auditList.map(x => { return x.auditor }).join(' / ')}}
                            </div>
                            <table v-else class="step-table">
                                <thead>
                                    <tr>
                                        <td class="step-table-user">{{ $ts('審批人') }}</td>
                                        <td class="step-table-status">{{ $ts('狀態') }}</td>
                                        <td class="step-table-date">{{ $ts('審批時間') }}</td>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr v-for="(data, aindex) in item.auditList" :key="aindex">
                                        <td>
                                            {{ data.auditor }}
                                        </td>
                                        <td>
                                            {{ data.auditStatus ? getAuditStatus(data.auditStatus) : '' }}
                                        </td>
                                        <td class="step-table-date">
                                            {{ data.auditDate || '' }}
                                        </td>
                                    </tr>
                                </tbody>
                            </table>

                            <!-- {{ $ts("狀態") }}： {{ getAuditStatus(item.auditStatus) }} -->
                        </div>
                        <div class="step-table-text" v-if="item.stepAttrType != 'cc'">
                            {{ $ts('審批意見') }}： {{ item.remark || '-' }}
                        </div>
                    </template>
                    <template v-else>
                        <div class="step-text">{{ $ts('審批人') }}：{{ item.auditor }}</div>
                        <div class="step-text">
                            {{ $ts('狀態') }}： {{ getAuditStatus(item.auditStatus) }}
                        </div>
                        <div class="step-text">{{ $ts('審批意見') }}： {{ item.remark || '-' }}</div>
                    </template>
                </div>
            </div>
        </div>
    </div>
</template>
<script setup>
import { ref } from 'vue'
const props = defineProps({
    workFlowSteps: {
        type: Array,
        default: () => {
            return []
        }
    },
    auditDic: {
        type: Array,
        default: () => {
            return []
        }
    }
})
const getAuditStatus = (key) => {
    return (
        props.auditDic.find((x) => {
            return x.key === key + ''
        }) || { value: key }
    ).value
}
</script>
<style scoped lang="less">
.step-item {
    background: #fff;
    display: flex;
}

.left-item {
    min-width: 152px;
    text-align: right;
    padding-right: 25px;
    padding-top: 8px;

    .left-date {
        font-size: 13px;
        padding-top: 7px;
        color: #484747;
    }
}

.right-item {
    cursor: pointer;
    position: relative;
    border-bottom: 1px solid #f3f3f3;
    padding: 5px 0 5px 5px;
}

.left-item,
.right-item {
    padding-bottom: 10px;
}

.right-item:last-child {
    border-bottom: 0;
}

.step-line {
    top: 16px;
    left: -10px;
    width: 1px;
    height: 100%;
    position: absolute;
    background-color: #ebedf0;
}

.step-circle {
    position: absolute;
    top: 17px;
    left: -9px;
    z-index: 2;
    font-size: 12px;
    line-height: 1;
    transform: translate(-50%, -50%);
    width: 7px;
    height: 7px;
    background-color: #a1a1a1;
    border-radius: 50%;
}

.right-item::before {
    content: '';
}

.step-content {
    padding-top: 2px;
    font-size: 14px;
    color: #828282;
    line-height: 1.5;
}

.step-title {
    font-weight: bold;
    padding-top: 3px;
}

.step-text {
    font-size: 13px;
    color: #999999;
    padding-top: 6px;
}

.step-current {
    * {
        color: #2f95ff !important;
    }

    .step-circle {
        background: #2f95ff !important;
    }

    // border-radius: 5px;
    // border: 1px solid #d6eaff;
    font-size: 13px;
    padding-top: 6px;
    // background-color: #eff7ffd9;
    color: black;
}

.step-table {
    border-collapse: collapse;
    border-spacing: 0;

    tr:first-child {
        background: #f5f7fa;
    }

    tr:last-child {
        border-bottom: none;
    }

    td {
        padding: 5px 8px;
        color: #444242;
        font-size: 12px;
        border-style: solid;
        border-width: 1px;
        border-color: #ebeef5;
    }

    .step-table-user {
        width: 80px;
    }

    .step-table-status {
        width: 80px;
    }

    .step-table-date {
        width: 130px;
    }

    thead td {
        color: #000 !important;
        font-weight: bold;
        font-size: 13px !important;
    }
}
.wl-icon{
    margin-left: 5px;
}
</style>