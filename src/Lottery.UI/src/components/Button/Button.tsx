import {
  ButtonVariant,
  Button as MantineButton,
  MantineColor,
  MantineGradient,
  useMantineTheme,
} from "@mantine/core";
import { useMediaQuery } from "@mantine/hooks";
import {
  ButtonHTMLAttributes,
  PropsWithChildren,
  ReactElement,
  type ReactNode,
} from "react";
import { isValidElement } from "react";

function isButtonProps(value: unknown): value is ButtonProps {
  if (
    value == null ||
    typeof value !== "object" ||
    isValidElement(value) ||
    Array.isArray(value)
  ) {
    return false;
  }

  return (
    "onClick" in value ||
    "disabled" in value ||
    "variant" in value ||
    "color" in value ||
    "fullWidth" in value
  );
}

export type ButtonType = ButtonHTMLAttributes<HTMLButtonElement>["type"];

interface ButtonProps {
  disabled?: boolean;
  type?: ButtonType;
  onClick?(): void;
  fullWidth?: boolean;
  variant?: ButtonVariant;
  color?: MantineColor;
  children?: ReactNode;
  formId?: string;
  gradient?: MantineGradient;
}

export default function Button({
  color,
  disabled,
  fullWidth,
  onClick,
  type,
  variant,
  children,
  formId,
  gradient,
}: ButtonProps) {
  const { breakpoints } = useMantineTheme();
  const sm = useMediaQuery(`(max-width: ${breakpoints.sm})`);
  return (
    <MantineButton
      disabled={disabled}
      type={type}
      onClick={onClick}
      fullWidth={fullWidth || sm}
      variant={variant}
      color={color}
      gradient={gradient}
      form={formId}
    >
      {children}
    </MantineButton>
  );
}

function CancelButton(props: Omit<ButtonProps, "variant" | "color" | "type">) {
  return (
    <Button {...props} variant="outline" color="red" type="reset">
      Cancel
    </Button>
  );
}

function SubmitButton(
  props: Omit<ButtonProps, "variant" | "gradient" | "type">,
) {
  return (
    <PrimaryButton {...props} type="submit">
      Submit
    </PrimaryButton>
  );
}

function PrimaryButton(
  props: PropsWithChildren<Omit<ButtonProps, "variant" | "gradient">>,
) {
  return (
    <Button
      {...props}
      variant="gradient"
      gradient={{ from: "blue", to: "grape", deg: 20 }}
    >
      {props.children}
    </Button>
  );
}

export interface ButtonPairProps {
  first?: ButtonProps | ReactElement<ButtonProps>;
  second?: ButtonProps | ReactElement<ButtonProps>;
}

export function ButtonPair({ first, second }: ButtonPairProps) {
  const { breakpoints } = useMantineTheme();
  const xs = useMediaQuery(`(max-width: ${breakpoints.xs})`);
  const sm = useMediaQuery(`(max-width: ${breakpoints.sm})`);
  return (
    <MantineButton.Group
      orientation={xs ? "vertical" : "horizontal"}
      w={sm ? "100%" : "auto"}
    >
      {isButtonProps(first) && isButtonProps(second) ? (
        <>
          <Button {...first} />
          <Button {...second} />
        </>
      ) : (
        <>
          {first}
          {second}
        </>
      )}
    </MantineButton.Group>
  );
}

interface CancelSubmitProps {
  cancelDisabled?: boolean;
  submitDisabled?: boolean;
  onCancelClicked?(): void;
  onSubmitClicked?(): void;
  form?: string | Partial<{ cancel: string; submit: string }>;
}

export function CancelSubmit({
  cancelDisabled,
  onCancelClicked,
  onSubmitClicked,
  submitDisabled,
  form,
}: CancelSubmitProps) {
  const [cancelFormId, submitFormId] =
    typeof form === "string" ? [form, form] : [form?.cancel, form?.submit];

  return (
    <ButtonPair
      first={
        <Button.Cancel
          formId={cancelFormId}
          disabled={cancelDisabled}
          onClick={onCancelClicked}
        />
      }
      second={
        <Button.Submit
          formId={submitFormId}
          disabled={submitDisabled}
          onClick={onSubmitClicked}
        />
      }
    />
  );
}

Button.Cancel = CancelButton;
Button.Submit = SubmitButton;
Button.Primary = PrimaryButton;
Button.Pair = ButtonPair;
ButtonPair.CancelSubmit = CancelSubmit;
