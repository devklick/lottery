import { useForm } from "react-hook-form";
import {
  SignInRequest,
  SignInResponse,
  signInRequestSchema,
} from "./signIn.schema";
import { zodResolver } from "@hookform/resolvers/zod";
import { useNavigate } from "react-router-dom";
import { useMutation } from "@tanstack/react-query";
import accountService from "../accountService";
import {
  Button,
  Checkbox,
  Group,
  InputError,
  InputLabel,
  PasswordInput,
  Stack,
  TextInput,
  Title,
} from "@mantine/core";
import { useUserStore } from "../../stores/userStore";

interface SignInProps {}

function SignIn({}: SignInProps) {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<SignInRequest>({
    resolver: zodResolver(signInRequestSchema),
  });

  const navigate = useNavigate();
  const userStore = useUserStore();

  function onSignInSuccess(response: SignInResponse) {
    userStore.login(response.userType, response.sessionExpiry);
    navigate("/home");
  }

  const mutation = useMutation<SignInResponse, unknown, SignInRequest>({
    mutationFn: async (request) => await accountService.signIn(request),
    onSuccess: onSignInSuccess,
  });

  function FieldError({ name }: { name: keyof SignInRequest }) {
    const message = errors[name]?.message;
    return message && <InputError>{message}</InputError>;
  }

  return (
    <form onSubmit={handleSubmit((data) => mutation.mutate(data))}>
      <Stack gap={24}>
        <Title>Sign In</Title>

        <TextInput placeholder="Username" {...register("username")} />
        <FieldError name="username" />

        <PasswordInput placeholder="Password" {...register("password")} />
        <FieldError name="password" />

        <Group>
          <Checkbox {...register("staySignedIn")} />
          <InputLabel>Stay signed in</InputLabel>
        </Group>
        <FieldError name="staySignedIn" />

        <Button variant="filled" type="submit">
          Submit
        </Button>
      </Stack>
    </form>
  );
}

export default SignIn;
