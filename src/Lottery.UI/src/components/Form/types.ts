import {
  GroupProps,
  StackProps,
  TextInputProps as _TextInputProps,
  PasswordInputProps as _PasswordInputProps,
  TextareaProps as _TextareaProps,
  NumberInputProps as _NumberInputProps,
  PinInputProps as _PinInputProps,
  InputWrapperProps,
  SelectProps as _SelectProps,
  MultiSelectProps as _MultiSelectProps,
  CheckboxGroupProps as _CheckboxGroupProps,
  CheckboxProps,
  RadioGroupProps as _RadioGroupProps,
  RadioProps,
  SwitchGroupProps as _SwitchGroupProps,
  SwitchProps as _SwitchProps,
} from "@mantine/core";
import { DateInputProps as _DateInputProps } from "@mantine/dates";
import React, { ReactNode } from "react";

export type Option<OtherProps = {}> = {
  label: ReactNode;
  value: any;
} & OtherProps;

export interface Options<OtherProps = {}> {
  options: Option<OtherProps>[];
}

export type Controlled<T> = { label: ReactNode } & T;
export type Orientation =
  | { orientation?: "horizontal"; orientationProps?: GroupProps }
  | { orientation?: "vertical"; orientationProps?: StackProps };
export type TextInputProps = Controlled<_TextInputProps>;
export type PasswordInputProps = Controlled<_PasswordInputProps>;
export type TextareaProps = Controlled<_TextareaProps>;
export type NumberInputProps = Controlled<_NumberInputProps>;
export type DateInputProps = Controlled<_DateInputProps>;
export type PinInputProps = Controlled<_PinInputProps> & InputWrapperProps;
export type SwitchProps = Controlled<_SwitchProps> & InputWrapperProps;

export type SelectProps = Controlled<
  Omit<_SelectProps, "data"> & {
    options: _SelectProps["data"];
  }
>;
export type MultiSelectProps = Controlled<
  Omit<_MultiSelectProps, "data"> & {
    options: _MultiSelectProps["data"];
  }
>;
export type CheckboxGroupProps = Controlled<
  Omit<_CheckboxGroupProps, "children"> & Options<CheckboxProps> & Orientation
>;
export type RadioGroupProps = Controlled<
  Omit<_RadioGroupProps, "children"> & Options<RadioProps> & Orientation
>;
export type SwitchGroupProps = Controlled<
  Omit<_SwitchGroupProps, "children"> & Options<SwitchProps> & Orientation
>;

export type ControllerProps =
  | ({ control: "checkbox-group" } & CheckboxGroupProps)
  | ({ control: "date-input" } & DateInputProps)
  | ({ control: "multi-select" } & MultiSelectProps)
  | ({ control: "number-input" } & NumberInputProps)
  | ({ control: "password-input" } & PasswordInputProps)
  | ({ control: "pin-input" } & PinInputProps)
  | ({ control: "radio-group" } & RadioGroupProps)
  | ({ control: "select" } & SelectProps)
  | ({ control: "switch" } & SwitchProps)
  | ({ control: "switch-group" } & SwitchGroupProps)
  | ({ control: "text-area" } & TextareaProps)
  | ({ control: "text-input" } & TextInputProps)
  | ({ control: "custom-element" } & { element: React.ReactNode });

export type WithName<T> = T & { name: string };
