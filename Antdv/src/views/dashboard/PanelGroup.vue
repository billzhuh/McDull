<template>
  <a-row :gutter="40" class="panel-group">
    <a-col :xs="12" :sm="12" :lg="6" class="card-pana-col">
      <div class="card-panel" @click="handleSetLineChartData('newVisitis')">
        <div class="card-panel-icon-wrapper icon-people" :width="56" :height="56">
          <!-- <svg-icon icon-class="peoples" class-name="card-panel-icon" /> -->
          <!-- <a-icon :component="peoples" class="peoples" :width="56" :height="56" /> -->
          <a-icon type="team"  :style="{ fontSize: '50px', color: '#faad14' }"/>
        </div>
        <div class="card-panel-description">
          <div class="card-panel-text">
            访客
          </div>
          <count-to :start-val="0" :end-val="102400" :duration="2600" class="card-panel-num" />
        </div>
      </div>
    </a-col>
    <a-col :xs="12" :sm="12" :lg="6" class="card-pana-col">
      <div class="card-panel" @click="handleSetLineChartData('messages')">
        <div class="card-panel-icon-wrapper icon-message">
          <!-- <svg-icon icon-class="message" class-name="card-panel-icon" /> -->
          <a-icon type="message" :style="{ fontSize: '50px', color: '#08c' }" />
        </div>
        <div class="card-panel-description">
          <div class="card-panel-text">
            消息
          </div>
          <count-to :start-val="0" :end-val="81212" :duration="3000" class="card-panel-num" />
        </div>
      </div>
    </a-col>
    <a-col :xs="12" :sm="12" :lg="6" class="card-pana-col">
      <div class="card-panel" @click="handleSetLineChartData('purchases')">
        <div class="card-panel-icon-wrapper icon-money">
          <!-- <svg-icon icon-class="money" class-name="card-panel-icon" /> -->
          <a-icon type="money-collect" :style="{ fontSize: '50px', color: '#f5222d' }"/>
        </div>
        <div class="card-panel-description">
          <div class="card-panel-text">
            金额
          </div>
          <count-to :start-val="0" :end-val="9280" :duration="3200" class="card-panel-num" />
        </div>
      </div>
    </a-col>
    <a-col :xs="12" :sm="12" :lg="6" class="card-pana-col">
      <div class="card-panel" @click="handleSetLineChartData('shoppings')">
        <div class="card-panel-icon-wrapper icon-shopping">
          <!-- <svg-icon icon-class="shopping" class-name="card-panel-icon" /> -->
          <a-icon type="shopping" :style="{ fontSize: '50px', color: '#52c41a' }"/>
        </div>
        <div class="card-panel-description">
          <div class="card-panel-text">
            订单
          </div>
          <count-to :start-val="0" :end-val="13600" :duration="3600" class="card-panel-num" />
        </div>
      </div>
    </a-col>
  </a-row>
</template>

<script>
import CountTo from 'vue-count-to'

export default {
  components: {
    CountTo
  },
  methods: {
    handleSetLineChartData(type) {
      this.$emit('handleSetLineChartData', type)
    }
  }
}
</script>

<style lang="scss" scoped>
.panel-group {
  margin-top: 18px;

  .card-pana-col {
    margin-bottom: 32px;
  }

  .card-panel {
    height: 108px;
    cursor: pointer;
    font-size: 12px;
    position: relative;
    overflow: hidden;
    color: #666;
    background: #fff;
    box-shadow: 4px 4px 40px rgba(0, 0, 0, .05);
    border-color: rgba(0, 0, 0, .05);

    &:hover {
      .card-panel-icon-wrapper {
        color: #fff;
      }

      .icon-people {
        background: #40c9c6;
      }

      .icon-message {
        background: #36a3f7;
      }

      .icon-money {
        background: #f4516c;
      }

      .icon-shopping {
        background: #34bfa3
      }
    }

    .icon-people {
      color: #40c9c6;
    }

    .icon-message {
      color: #36a3f7;
    }

    .icon-money {
      color: #f4516c;
    }

    .icon-shopping {
      color: #34bfa3
    }

    .card-panel-icon-wrapper {
      float: left;
      margin: 14px 0 0 14px;
      padding: 16px;
      transition: all 0.38s ease-out;
      border-radius: 6px;
    }

    .card-panel-icon {
      float: left;
      font-size: 48px;
    }

    .card-panel-description {
      float: right;
      font-weight: bold;
      margin: 26px;
      margin-left: 0px;

      .card-panel-text {
        line-height: 18px;
        color: rgba(0, 0, 0, 0.45);
        font-size: 16px;
        margin-bottom: 12px;
      }

      .card-panel-num {
        font-size: 20px;
      }
    }
  }
}

@media (max-width:550px) {
  .card-panel-description {
    display: none;
  }

  .card-panel-icon-wrapper {
    float: none !important;
    width: 100%;
    height: 100%;
    margin: 0 !important;

    .svg-icon {
      display: block;
      margin: 14px auto !important;
      float: none !important;
    }
  }
}
</style>
