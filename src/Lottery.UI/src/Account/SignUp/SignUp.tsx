import { useForm } from "@mantine/form";
import { zodResolver } from "mantine-form-zod-resolver";
import {
  SignUpRequest,
  SignUpResponse,
  signUpRequestSchema,
} from "./signUp.schema";
import { Button, PasswordInput, Stack, TextInput, Title } from "@mantine/core";
import { useMutation } from "@tanstack/react-query";
import accountService from "../accountService";
import { useNavigate } from "react-router-dom";

interface SignUpProps {}

function SignUp({}: SignUpProps) {
  const form = useForm<SignUpRequest>({
    validate: zodResolver(signUpRequestSchema),
    validateInputOnChange: true,
  });

  const navigate = useNavigate();

  const mutation = useMutation<SignUpResponse, unknown, SignUpRequest>({
    mutationFn: async (request) => await accountService.signUp(request),
    onSuccess: () => navigate("/home"),
  });

  return (
    <form onSubmit={form.onSubmit((data) => mutation.mutate(data))}>
      <Stack gap={24}>
        <Title>Sign Up</Title>

        <TextInput placeholder="Email" {...form.getInputProps("email")} />

        <TextInput placeholder="Username" {...form.getInputProps("username")} />

        <PasswordInput
          placeholder="Password"
          {...form.getInputProps("password")}
        />

        <PasswordInput
          placeholder="Confirm Password"
          {...form.getInputProps("confirmPassword")}
        />

        <Button variant="filled" type="submit">
          Submit
        </Button>
      </Stack>
    </form>
  );
}

export default SignUp;
