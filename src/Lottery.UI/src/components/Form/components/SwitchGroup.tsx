import { Group, Stack, Switch } from "@mantine/core";
import { useController } from "react-hook-form";
import { SwitchGroupProps, WithName } from "../types";
import ErrorMessage from "./ErrorMessage";

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

  const error = fieldError ? (
    <ErrorMessage>{fieldError.message?.toString()}</ErrorMessage>
  ) : undefined;

  const { onChange, ...restField } = field;

  return (
    <Switch.Group
      id={name}
      label={label}
      error={error}
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
