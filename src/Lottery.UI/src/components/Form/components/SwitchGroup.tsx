import { Group, Stack, Switch } from "@mantine/core";
import { useController } from "react-hook-form";
import { SwitchGroupProps, WithName } from "../types";

function SwitchGroup(props: WithName<SwitchGroupProps>) {
  const {
    label,
    name,
    options,
    orientation = "horizontal",
    orientationProps,
    ...rest
  } = props;
  const {
    field,
    fieldState: { error: fieldError },
    formState: { defaultValues },
  } = useController({ name });

  const { onChange, ...restField } = field;

  return (
    <Switch.Group
      id={name}
      label={label}
      error={fieldError?.message?.toString()}
      onChange={(value) => {
        onChange(value ?? defaultValues?.[name]);
      }}
      {...rest}
      {...restField}
    >
      {/* TODO: Reduce duplication */}
      {orientation == "horizontal" ? (
        <Group mt={"xs"}>
          {options.map((option, index) => {
            const { label, value, ...rest } = option;
            return (
              <Switch
                key={`${label}-${index}`}
                value={value}
                label={label}
                {...rest}
              />
            );
          })}
        </Group>
      ) : (
        <Stack mt={"xs"}>
          {options.map((option, index) => {
            const { label, value, ...rest } = option;
            return (
              <Switch
                key={`${label}-${index}`}
                value={value}
                label={label}
                {...rest}
              />
            );
          })}
        </Stack>
      )}
    </Switch.Group>
  );
}

export default SwitchGroup;
