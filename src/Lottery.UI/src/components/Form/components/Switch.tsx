import { useController } from "react-hook-form";
import { SwitchProps, WithName } from "../types";
import { Switch as _Switch } from "@mantine/core";

function Switch(props: WithName<SwitchProps>) {
  const { label, name, withAsterisk, ...rest } = props;
  const {
    field,
    fieldState: { error: fieldError },
    formState: { defaultValues },
  } = useController({ name });

  return <_Switch key={`${label}`} label={label} {...rest} />;
}

export default Switch;
