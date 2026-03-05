import { h } from 'vue';

export default {
  name: "FormExpand",
  functional: true,
  props: {
    render: Function,
    par: {}//測試参數
  },
  render: ({ render, par }) => {
    return render(h, { par }); //h();
  }
};

